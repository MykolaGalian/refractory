using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.Api.Models.Account;
using RefractoryApp.Api.Services;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IBLLUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;

    public AccountController(
        IBLLUnitOfWork uow, 
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        JwtService jwtService)
    {
        _uow = uow;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    [Authorize]
    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        var userName = _userManager.GetUserName(User);
        // Note: JWT tokens are stateless, so server-side logout just acknowledges the request
        // The client should discard the token
        return Ok($"{userName} logged out");
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId != null)
        {
            return BadRequest($"{_userManager.GetUserName(User)} already logged in.");
        }

        if (model == null)
        {
            return BadRequest("Invalid user data.");
        }

        if (await _uow.UserInfoService.CheckLoginExistAsync(model.Login))
        {
            ModelState.AddModelError("Login", "Login is already taken.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<RegisterViewModel, DTOUser>(model);
        user.Roles = new List<string> { "user" };
        user.DateRegistration = DateTime.Now;

        var result = await _uow.UserManagerService.CreateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return Ok($"{user.Login} user registered ok.");
    }
}