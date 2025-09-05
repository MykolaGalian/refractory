using RefractoryApp.BLL.Contracts;
using RefractoryApp.DAL.Contracts;

namespace RefractoryApp.BLL.Services;

public class ModeratorService : IModeratorService
{
    private readonly IUnitOfWork _uow;

    public ModeratorService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task BlockRefractoryAsync(int refId, string userName)
    {
        if (refId <= 0)
            throw new ArgumentException("Refractory ID must be greater than 0", nameof(refId));

        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("User name is required", nameof(userName));

        var refractory = await _uow.Refractories.SelectByIdAsync(refId);
        if (refractory == null)
            throw new ArgumentException($"Refractory with ID {refId} not found");

        if (refractory.IsBlocked)
            throw new InvalidOperationException("Refractory is already blocked");

        refractory.IsBlocked = true;
        await _uow.Refractories.UpdateAsync(refractory);
        await _uow.SaveAsync();
    }

    public async Task UnblockRefractoryAsync(int refId)
    {
        if (refId <= 0)
            throw new ArgumentException("Refractory ID must be greater than 0", nameof(refId));

        var refractory = await _uow.Refractories.SelectByIdAsync(refId);
        if (refractory == null)
            throw new ArgumentException($"Refractory with ID {refId} not found");

        if (!refractory.IsBlocked)
            throw new InvalidOperationException("Refractory is not blocked");

        refractory.IsBlocked = false;
        await _uow.Refractories.UpdateAsync(refractory);
        await _uow.SaveAsync();
    }

    public void Dispose()
    {
        _uow?.Dispose();
    }
}