using BulkkyBook.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace BulkkyBook.Security;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionAuthorizationHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = context.User;
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return Task.CompletedTask;
        }


        var userRoles = user.Claims
            .Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(x => x.Value)
            .ToList();


        var hasPermission = userRoles.Any(role =>
        {
            var permissions = _unitOfWork.Permission.GetPermissionsByRole(role);
            return permissions.Any(p =>
            p.FunctionId == requirement.FunctionId &&
            p.CommandId == requirement.CommandId);
        });

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}