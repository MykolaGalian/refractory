using RefractoryApp.BLL.Contracts;
using RefractoryApp.DAL.Contracts;

namespace RefractoryApp.BLL.Services;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _uow;

    public AdminService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task BlockAccountAsync(int userId, string adminLogin)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(userId));

        if (string.IsNullOrWhiteSpace(adminLogin))
            throw new ArgumentException("Admin login is required", nameof(adminLogin));

        var user = await _uow.UserInfos.SelectByIdAsync(userId);
        if (user == null)
            throw new ArgumentException($"User with ID {userId} not found");

        if (user.IsBlocked)
            throw new InvalidOperationException("User is already blocked");

        user.IsBlocked = true;
        await _uow.UserInfos.UpdateAsync(user);
        await _uow.SaveAsync();
    }

    public async Task UnblockAccountAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(userId));

        var user = await _uow.UserInfos.SelectByIdAsync(userId);
        if (user == null)
            throw new ArgumentException($"User with ID {userId} not found");

        if (!user.IsBlocked)
            throw new InvalidOperationException("User is not blocked");

        user.IsBlocked = false;
        await _uow.UserInfos.UpdateAsync(user);
        await _uow.SaveAsync();
    }

    public void Dispose()
    {
        _uow?.Dispose();
    }
}