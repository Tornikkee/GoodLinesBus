using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using AdminPanel.Models;
using AdminPanel.Models.ViewModels;

[Authorize(Roles = "Admin")]
public class ManagementController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DapperRepository _dapperRepository;

    public ManagementController(UserManager<ApplicationUser> userManager, DapperRepository dapperRepository)
    {
        _userManager = userManager;
        _dapperRepository = dapperRepository;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _dapperRepository.GetAllUsersAsync();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var model = new EditUserViewModel { Id = user.Id, Email = user.Email };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.Email = model.Email;
            user.UserName = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
        }
        return NotFound();
    }
}
