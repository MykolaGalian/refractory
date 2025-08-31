namespace RefractoryApp.BLL.Contracts;

public interface IAdminService : IDisposable
{
    Task BlockAccountAsync(int userId, string adminLogin);
    Task UnblockAccountAsync(int userId);
}