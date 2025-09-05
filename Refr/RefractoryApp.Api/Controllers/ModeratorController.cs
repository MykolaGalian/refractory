using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[Authorize(Roles = "moderator,admin")]
[ApiController]
[Route("api/[controller]")]
public class ModeratorController : ControllerBase
{
    private readonly IBLLUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;

    public ModeratorController(IBLLUnitOfWork uow, UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _userManager = userManager;
    }

    [HttpGet("block/refractory/{refId}")]
    public async Task<IActionResult> BlockRefractory(int refId)
    {
        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        if (refractory.IsBlocked)
            return BadRequest("Refractory already blocked.");

        var userName = _userManager.GetUserName(User);
        if (userName == null)
            return Unauthorized();

        await _uow.ModeratorService.BlockRefractoryAsync(refId, userName);
        return Ok("Refractory blocked");
    }

    [HttpGet("unblock/refractory/{refId}")]
    public async Task<IActionResult> UnblockRefractory(int refId)
    {
        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        if (!refractory.IsBlocked)
            return BadRequest("Refractory not blocked.");

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null)
            return Unauthorized();

        await _uow.ModeratorService.UnblockRefractoryAsync(refId);
        return Ok("Refractory unblocked");
    }
}