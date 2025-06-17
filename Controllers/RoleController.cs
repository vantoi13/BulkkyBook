using BulkkyBook.Security;
using BulkkyBook.Utils.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BulkkyBook.Controllers;

// [Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [Permission(Constants.Functions.SystemRole, Constants.Commands.Read)]
    public IActionResult Index()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Create)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Create)]
    public async Task<IActionResult> Create(IdentityRole role)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(role);
    }
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Update)]
    public IActionResult Edit(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }
    [HttpPost]
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(string id, IdentityRole role)
    {
        if (id != role.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound();
            }

            existingRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(existingRole);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(role);
    }
    [HttpPost]
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        if (role.Name!.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Cannot delete the Admin role.");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }
}
