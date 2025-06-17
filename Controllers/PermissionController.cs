using BulkkyBook.Data.Entities;
using BulkkyBook.DTOS.Identity;
using BulkkyBook.Repositories;
using BulkkyBook.Utils.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkkyBook.Controllers;

public class PermissionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PermissionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index(string roleId)
    {
        var role = _unitOfWork.Role.GetRoles().FirstOrDefault(r => r.Id == roleId);
        if (role == null)
        {
            return NotFound();
        }
        var permissions = _unitOfWork.Permission.GetPermissionsByRole(roleId);
        var functions = _unitOfWork.Function.GetFunctions();
        var commands = _unitOfWork.Command.GetCommands();

        var funtionViewModel = functions.Select(f => new FunctionViewModel
        {
            Id = f.Id,
            Name = f.Name,
            ParentId = f.ParentId,
            HasPermission = permissions.Any(p => p.FunctionId == f.Id),
            Commands = commands.Select(c => new CommandViewModel
            {
                Id = c.Id,
                Name = c.Name,
                IsSelected = permissions.Any(p => p.FunctionId == f.Id && p.CommandId == c.Id)
            }).ToList()

        }).ToList();
        var viewModel = new PermissionViewModel
        {
            RoleId = role.Id,
            RoleName = role.Name,
            Functions = funtionViewModel
        };

        return View(viewModel);
    }
    [HttpPost]
    public IActionResult SavePermissions(PermissionViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index), new { roleId = model.RoleId });


        var newPermissions = new List<Permission>();
        foreach (var function in model.Functions!)
        {
            foreach (var command in function.Commands!)
            {
                if (command.IsSelected)
                {
                    newPermissions.Add(new Permission
                    {
                        RoleId = model.RoleId,
                        FunctionId = function.Id,
                        CommandId = command.Id
                    });
                }
            }
        }
        _unitOfWork.Permission.RemovePermissions(model.RoleId!);
        _unitOfWork.Permission.SavePermissions(model.RoleId!, newPermissions);
        return RedirectToAction(nameof(Index), new { roleId = model.RoleId });
    }

}