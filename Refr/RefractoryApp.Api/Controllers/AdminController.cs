using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IBLLUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(IBLLUnitOfWork uow, UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _userManager = userManager;
    }


    [HttpGet("block/user/{accountLogin}")]
    public async Task<IActionResult> BlockAccount(string accountLogin)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _uow.UserInfoService.GetUserByLoginAsync(accountLogin);
            if (user == null)
                return NotFound($"User {accountLogin} not found");

            if (user.IsBlocked)
                return BadRequest("User already blocked.");

            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
                return Unauthorized();

            var admin = await _uow.UserInfoService.GetUserByIdAsync(int.Parse(currentUserId));
            if (admin == null)
                return BadRequest("Admin not found");

            await _uow.AdminService.BlockAccountAsync(user.Id, admin.Login);
            return Ok("Account blocked");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpGet("unblock/user/{accountLogin}")]
    public async Task<IActionResult> UnblockAccount(string accountLogin)
    {
        var user = await _uow.UserInfoService.GetUserByLoginAsync(accountLogin);
        if (user == null)
            return NotFound();

        if (!user.IsBlocked)
            return BadRequest("User is not blocked.");

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null)
            return Unauthorized();

        await _uow.AdminService.UnblockAccountAsync(user.Id);
        return Ok("Account unblocked");
    }
}