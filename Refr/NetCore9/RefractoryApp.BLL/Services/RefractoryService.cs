using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.BLL.Services;

public class RefractoryService : IRefractoryService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RefractoryService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public void Dispose()
    {
        _uow?.Dispose();
    }

    public async Task<List<DTORefractory>> GetAllRefractoriesAsync()
    {
        var refractories = (await _uow.Refractories.SelectAllAsync())
            .OrderByDescending(x => x.DateCreate);

        return _mapper.Map<List<DTORefractory>>(refractories);
    }

    public async Task AddRefractoryAsync(DTORefractory dtoRefractory)
    {
        if (string.IsNullOrWhiteSpace(dtoRefractory.RefractoryDescription))
            throw new ArgumentException("Refractory description is required", nameof(dtoRefractory.RefractoryDescription));

        var refractory = _mapper.Map<Refractory>(dtoRefractory);
        refractory.DateCreate = DateTime.Now;
        
        await _uow.Refractories.InsertAsync(refractory);
        await _uow.SaveAsync();
    }

    public async Task EditRefractoryAsync(DTORefractory dtoRefractory)
    {
        if (dtoRefractory.UserInfoId == 0)
            throw new ArgumentException("UserInfoId is required", nameof(dtoRefractory.UserInfoId));

        var refractory = _mapper.Map<Refractory>(dtoRefractory);
        refractory.LastEdit = DateTime.Now;
        
        await _uow.Refractories.UpdateAsync(refractory);
        await _uow.SaveAsync();
    }

    public async Task DeleteRefractoryAsync(int refId)
    {
        // Сначала удаляем связанные комментарии
        var comments = await _uow.Comments.SelectAllAsync(x => x.RefractoryId == refId);
        foreach (var comment in comments)
        {
            await _uow.Comments.DeleteAsync(comment.Id);
        }

        await _uow.Refractories.DeleteAsync(refId);
        await _uow.SaveAsync();
    }

    public async Task<DTORefractory?> GetRefractoryByRefIdAsync(int refId)
    {
        var refractory = await _uow.Refractories.SelectByIdAsync(refId);
        if (refractory == null) return null;

        // Загружаем комментарии
        var comments = await _uow.Comments.SelectAllAsync(x => x.RefractoryId == refId);
        refractory.Comments = comments.ToList();

        return _mapper.Map<DTORefractory>(refractory);
    }

    public async Task<DTORefractory?> GetRefractoryByBrandAsync(string refBrand)
    {
        var refractories = await _uow.Refractories.SelectAllAsync(x => x.RefractoryBrand == refBrand);
        var refractory = refractories.FirstOrDefault();
        
        return refractory != null ? _mapper.Map<DTORefractory>(refractory) : null;
    }

    public async Task<IEnumerable<DTORefractory>> GetRefractoriesByUserIdAsync(int userid)
    {
        var refractories = await _uow.Refractories.SelectAllAsync(x => x.UserInfoId == userid);
        var sortedRefractories = refractories.OrderByDescending(x => x.DateCreate);
        
        return _mapper.Map<IEnumerable<DTORefractory>>(sortedRefractories);
    }

    public async Task<IEnumerable<DTORefractory>> GetRefractoriesByTypeAsync(string refType)
    {
        var refractories = await _uow.Refractories.SelectAllAsync(x => x.RefractoryType == refType);
        var sortedRefractories = refractories.OrderByDescending(x => x.DateCreate);
        
        return _mapper.Map<IEnumerable<DTORefractory>>(sortedRefractories);
    }

    public async Task<IEnumerable<string>> GetAllRefTypesAsync()
    {
        var refractories = await _uow.Refractories.SelectAllAsync();
        
        return refractories
            .Where(x => !string.IsNullOrEmpty(x.RefractoryType))
            .Select(x => x.RefractoryType!)
            .Distinct()
            .ToList();
    }
}