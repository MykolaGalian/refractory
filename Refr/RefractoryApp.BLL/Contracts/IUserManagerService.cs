using Microsoft.AspNetCore.Identity;
using RefractoryApp.BLL.DTO;
using System.Security.Claims;

namespace RefractoryApp.BLL.Contracts;

public interface IUserManagerService
{
    Task<IdentityResult> CreateAsync(DTOUser user);
    Task<ClaimsIdentity?> GetClaimAsync(string username, string password);
    Task<DTOUser?> GetUserByLoginAsync(string login);
    Task<bool> IsUserInRoleAdminAsync(int userId);
    Task<bool> IsUserInRoleModeratorAsync(int userId);
}