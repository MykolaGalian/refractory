using RefractoryApp.BLL.DTO;

namespace RefractoryApp.BLL.Contracts;

public interface IUserInfoService : IDisposable
{
    Task<List<DTOUser>> GetAllUserInfoAsync();
    Task UpdateAsync(DTOUser user);
    Task DeleteAsync(int userId);
    Task SetUserAvatarAsync(int userId, string avatar);
    Task<DTOUser?> GetUserByIdAsync(int id);
    Task<DTOUser?> GetUserByLoginAsync(string login);
    Task<bool> CheckLoginExistAsync(string login);
}