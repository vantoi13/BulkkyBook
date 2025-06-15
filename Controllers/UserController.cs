using BulkkyBook.Data.Entities;
using BulkkyBook.DTOS.Identity;
using BulkkyBook.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkkyBook.Controllers;

public class UserController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<User> _signInManager;

    public UserController(IUnitOfWork unitOfWork, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }
    public IActionResult Index()
    {
        var users = _unitOfWork.User.GetUsers();
        return View(users);
    }
    public IActionResult Edit(string id)
    {
        var user = _unitOfWork.User.GetUser(id);
        var roles = _unitOfWork.Role.GetRoles();
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = _signInManager.UserManager.GetRolesAsync(user);
        var roleItems = roles.Select(role => new SelectListItem(
            role.Name,
            role.Name,
            userRoles.Result.Contains(role.Name!)
        )).ToList();

        var vm = new EditUserViewModels
        {
            User = user,
            Roles = roleItems
        };

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> OnPostAsync(EditUserViewModels data)
    {
        var user = _unitOfWork.User.GetUser(data.User!.Id);
        if (user == null)
        {
            return NotFound();
        }
        var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

        var rolesToAdd = data.Roles!.Where(role => role.Selected && !userRoles.Contains(role.Text!)).Select(role => role.Text!).ToList();
        var rolesToRemove = data.Roles!.Where(role => !role.Selected && userRoles.Contains(role.Text!)).Select(role => role.Text!).ToList();
        if (rolesToAdd.Any())
        {
            await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
        }
        if (rolesToRemove.Any())
        {
            await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToRemove);
        }
        user.FirstName = data.User.FirstName;
        user.LastName = data.User.LastName;
        user.Email = data.User.Email;

        _unitOfWork.User.UpdateUser(user);

        return RedirectToAction(nameof(Index));

    }
}