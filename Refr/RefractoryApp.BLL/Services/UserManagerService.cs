using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;
using System.Security.Claims;

namespace RefractoryApp.BLL.Services;

public class UserManagerService : IUserManagerService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerService(IUnitOfWork uow, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(DTOUser dtoUser)
    {
        if (dtoUser == null)
            throw new ArgumentNullException(nameof(dtoUser));

        // Проверяем, существует ли пользователь с таким email
        var existingUser = await _userManager.FindByEmailAsync(dtoUser.Email);
        if (existingUser != null)
        {
            return IdentityResult.Failed(new IdentityError 
            { 
                Code = "DuplicateEmail", 
                Description = "User with this email already exists" 
            });
        }

        // Создаем ApplicationUser и UserInfo
        var user = new ApplicationUser
        {
            Email = dtoUser.Email,
            UserName = dtoUser.Login,
            UserInfo = new UserInfo
            {
                Name = dtoUser.Name,
                LastName = dtoUser.LastName,
                Login = dtoUser.Login,
                UserAvatar = dtoUser.UserAvatar,
                Position = dtoUser.Position,
                IsBlocked = false,
                DateRegistration = DateTime.Now
            }
        };

        // Создаем пользователя
        var result = await _userManager.CreateAsync(user, dtoUser.Password);
        if (!result.Succeeded)
            return result;

        // Добавляем роль (по умолчанию "user")
        var role = dtoUser.Roles.FirstOrDefault() ?? "user";
        await _userManager.AddToRoleAsync(user, role);

        await _uow.SaveAsync();
        return IdentityResult.Success;
    }

    public async Task<ClaimsIdentity?> GetClaimAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return null;

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return null;

        // Создаем ClaimsIdentity для JWT
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new(ClaimTypes.Email, user.Email ?? "")
        };

        // Добавляем роли пользователя
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new ClaimsIdentity(claims, "Bearer");
    }

    public async Task<DTOUser?> GetUserByLoginAsync(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return null;

        var user = await _userManager.FindByNameAsync(login);
        if (user?.UserInfo == null)
            return null;

        var dtoUser = _mapper.Map<DTOUser>(user.UserInfo);
        dtoUser.Email = user.Email ?? "";
        dtoUser.Roles = (await _userManager.GetRolesAsync(user)).ToList();

        return dtoUser;
    }

    public async Task<bool> IsUserInRoleAdminAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user != null && await _userManager.IsInRoleAsync(user, "admin");
    }

    public async Task<bool> IsUserInRoleModeratorAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user != null && await _userManager.IsInRoleAsync(user, "moderator");
    }
}