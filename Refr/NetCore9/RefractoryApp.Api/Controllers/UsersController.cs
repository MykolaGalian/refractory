using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.Api.Models.Refractory;
using RefractoryApp.Api.Models.User;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IBLLUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public UsersController(
        IBLLUnitOfWork uow, 
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _uow = uow;
        _userManager = userManager;
        _mapper = mapper;
        _env = env;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> MyProfile()
    {
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return Unauthorized();

        var profile = await _uow.UserInfoService.GetUserByIdAsync(userId);
        if (profile == null)
            return Unauthorized();

        var emailData = await _uow.UserManagerService.GetUserByLoginAsync(profile.Login);
        if (emailData != null)
            profile.Email = emailData.Email;

        if (profile.IsBlocked)
            return BadRequest("Your account blocked.");

        var profileView = _mapper.Map<DTOUser, ProfileViewModel>(profile);

        var posts = await _uow.RefractoryService.GetRefractoriesByUserIdAsync(profile.Id);
        var filteredPosts = posts.Where(x => !x.IsBlocked).ToList();

        profileView.Refractories = _mapper.Map<List<RefractoryViewModel>>(filteredPosts);
        profileView.IsAdmin = User.IsInRole("admin");
        profileView.IsModerator = User.IsInRole("moderator");

        return Ok(profileView);
    }

    [HttpGet("allprofiles")]
    public async Task<IActionResult> AllProfiles()
    {
        var profiles = await _uow.UserInfoService.GetAllUserInfoAsync();
        if (profiles == null)
            return BadRequest(ModelState);

        var profileViews = _mapper.Map<List<ProfileViewModel>>(profiles);
        return Ok(profileViews);
    }

    [HttpPost("avatar/set")]
    public async Task<IActionResult> SetAvatar(IFormFile image)
    {
        var currentUserId = _userManager.GetUserId(User);
        var currentUserName = _userManager.GetUserName(User);
        
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return BadRequest("Not authorized");

        if (image == null || image.Length == 0)
            return BadRequest("No image");
        var userLogin = currentUserName!;

        var imageName = Path.GetFileNameWithoutExtension(image.FileName)[..Math.Min(5, Path.GetFileNameWithoutExtension(image.FileName).Length)] + Path.GetExtension(image.FileName);

        var userImageDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "Images", userLogin);
        if (!Directory.Exists(userImageDir))
            Directory.CreateDirectory(userImageDir);

        var filePath = Path.Combine(userImageDir, imageName);
        
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        await _uow.UserInfoService.SetUserAvatarAsync(userId, imageName);
        return Ok("avatar is set");
    }

    [HttpGet("image/get")]
    [AllowAnonymous]
    
    public IActionResult GetImage(string imageName, string userLogin)
    {
        var imagePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "Images", userLogin, imageName);
        
        if (!System.IO.File.Exists(imagePath))
            return NotFound();

        var imageBytes = System.IO.File.ReadAllBytes(imagePath);
        return File(imageBytes, "application/octet-stream");
    }

    [HttpPut("profile/edit")]
    public async Task<IActionResult> EditProfile([FromBody] ChangeProfileViewModel newProfile)
    {
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return Unauthorized();
        if (userId != newProfile.Id)
            return BadRequest("It's not your profile!");

        var user = await _uow.UserInfoService.GetUserByIdAsync(userId);
        if (user == null || user.IsBlocked)
            return BadRequest("Your account has been blocked.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var profileEdit = _mapper.Map<ChangeProfileViewModel, DTOUser>(newProfile);
        await _uow.UserInfoService.UpdateAsync(profileEdit);

        return Ok("Profile has been updated.");
    }

    [HttpDelete("profile/edit")]
    public async Task<IActionResult> DeleteProfile()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return Unauthorized();

        await _uow.UserInfoService.DeleteAsync(userId);
        return Ok("Profile deleted.");
    }
}