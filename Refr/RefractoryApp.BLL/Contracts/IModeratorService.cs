namespace RefractoryApp.BLL.Contracts;

public interface IModeratorService : IDisposable
{
    Task BlockRefractoryAsync(int refId, string userName);
    Task UnblockRefractoryAsync(int refId);
}