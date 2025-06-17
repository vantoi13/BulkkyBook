using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BulkkyBook.Security;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{   
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }    
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => 
        FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => 
        FallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission:", StringComparison.OrdinalIgnoreCase))
        {
            var permission = policyName.Substring("Permission:".Length); // SYSTEM_ROLE.CREATE
            var parts = permission.Split('.');

            if (parts.Length == 2)
            {
                var builder = new AuthorizationPolicyBuilder();
                builder.AddRequirements(new PermissionRequirement(parts[0], parts[1]));
                return Task.FromResult<AuthorizationPolicy?>(builder.Build());
            }
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }

}
