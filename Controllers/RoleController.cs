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
                TempData["Success"] = $"Tạo vai trò '{role.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            TempData["Error"] = "Có lỗi xảy ra khi tạo vai trò.";
        }
        return View(role);
    }
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Update)]
    public IActionResult Edit(string id)
    {
        var role = _roleManager.FindByIdAsync(id).Result;
        if (role == null)
        {
            TempData["Error"] = "Không tìm thấy vai trò.";
            return RedirectToAction(nameof(Index));
        }
        return View(role);
    }
    [HttpPost]
    [Permission(Constants.Functions.SystemRole, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(string id, IdentityRole role)
    {
        if (id != role.Id)
        {
            TempData["Error"] = "ID vai trò không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }
        if (ModelState.IsValid)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole == null)
            {
                TempData["Error"] = "Không tìm thấy vai trò.";
                return RedirectToAction(nameof(Index));
            }

            existingRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(existingRole);

            if (result.Succeeded)
            {
                TempData["Success"] = $"Cập nhật vai trò '{role.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            TempData["Error"] = "Có lỗi xảy ra khi cập nhật vai trò.";
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
            TempData["Error"] = "Không tìm thấy vai trò.";
            return RedirectToAction(nameof(Index));
        }
        if (role.Name!.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Không thể xóa vai trò Admin.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            TempData["Error"] = "Có lỗi xảy ra khi xóa vai trò.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = $"Xóa vai trò '{role.Name}' thành công!";
        return RedirectToAction(nameof(Index));
    }
}
