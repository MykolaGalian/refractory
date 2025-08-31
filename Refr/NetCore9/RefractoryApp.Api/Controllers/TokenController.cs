using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.Api.Services;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[ApiController]
[Route("")]
public class TokenController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtService _jwtService;

    public TokenController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Token([FromForm] string username, [FromForm] string password, [FromForm] string grant_type)
    {
        if (grant_type != "password")
        {
            return BadRequest(new { error = "unsupported_grant_type", error_description = "The authorization grant type is not supported by the authorization server." });
        }

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return BadRequest(new { error = "invalid_request", error_description = "Username and password are required." });
        }

        // Find user by username
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return BadRequest(new { error = "invalid_grant", error_description = "The user name or password is incorrect." });
        }

        // Check password
        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            return BadRequest(new { error = "invalid_grant", error_description = "The user name or password is incorrect." });
        }

        // Check if user is blocked (if UserInfo exists)
        if (user.UserInfo != null && user.UserInfo.IsBlocked)
        {
            return BadRequest(new { error = "invalid_grant", error_description = "User account is blocked." });
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        // Generate JWT token
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName ?? "", roles);

        return Ok(new
        {
            access_token = token,
            token_type = "Bearer",
            expires_in = 1209600 // 14 days in seconds
        });
    }
}