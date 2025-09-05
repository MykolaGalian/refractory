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
        // Получаем контекст через UnitOfWork для прямых запросов с Include
        var context = _uow.Context;
        var refractories = await context.Refractories
            .Include(r => r.UserInfo)
            .Include(r => r.Comments)
            .ThenInclude(c => c.UserInfo)
            .OrderByDescending(x => x.DateCreate)
            .ToListAsync();

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

        // Получаем существующую сущность и обновляем её поля
        var existingRefractory = await _uow.Refractories.SelectByIdAsync(dtoRefractory.Id);
        if (existingRefractory == null)
            throw new ArgumentException("Refractory not found", nameof(dtoRefractory.Id));

        existingRefractory.RefractoryDescription = dtoRefractory.RefractoryDescription;
        existingRefractory.RefractoryBrand = dtoRefractory.RefractoryBrand;
        existingRefractory.RefractoryType = dtoRefractory.RefractoryType;
        existingRefractory.Density = dtoRefractory.Density;
        existingRefractory.MaxWorkTemperature = dtoRefractory.MaxWorkTemperature;
        existingRefractory.Lime = dtoRefractory.Lime;
        existingRefractory.Alumina = dtoRefractory.Alumina;
        existingRefractory.Silica = dtoRefractory.Silica;
        existingRefractory.Magnesia = dtoRefractory.Magnesia;
        existingRefractory.Carbon = dtoRefractory.Carbon;
        existingRefractory.Price = dtoRefractory.Price;
        existingRefractory.LastEdit = DateTime.Now;
        
        await _uow.Refractories.UpdateAsync(existingRefractory);
        await _uow.SaveAsync();
    }

    public async Task DeleteRefractoryAsync(int refId)
    {
        // Сначала удаляем связанные комментарии через прямой запрос
        var context = _uow.Context;
        var comments = await context.Comments
            .Where(x => x.RefractoryId == refId)
            .ToListAsync();
            
        foreach (var comment in comments)
        {
            await _uow.Comments.DeleteAsync(comment.Id);
        }

        await _uow.Refractories.DeleteAsync(refId);
        await _uow.SaveAsync();
    }

    public async Task<DTORefractory?> GetRefractoryByRefIdAsync(int refId)
    {
        var context = _uow.Context;
        var refractory = await context.Refractories
            .Include(r => r.UserInfo)
            .Include(r => r.Comments)
            .ThenInclude(c => c.UserInfo)
            .FirstOrDefaultAsync(r => r.Id == refId);
            
        if (refractory == null) return null;

        return _mapper.Map<DTORefractory>(refractory);
    }

    public async Task<DTORefractory?> GetRefractoryByBrandAsync(string refBrand)
    {
        var context = _uow.Context;
        var refractory = await context.Refractories
            .Include(r => r.UserInfo)
            .Include(r => r.Comments)
            .ThenInclude(c => c.UserInfo)
            .FirstOrDefaultAsync(x => x.RefractoryBrand == refBrand);
        
        return refractory != null ? _mapper.Map<DTORefractory>(refractory) : null;
    }

    public async Task<IEnumerable<DTORefractory>> GetRefractoriesByUserIdAsync(int userid)
    {
        var context = _uow.Context;
        var refractories = await context.Refractories
            .Include(r => r.UserInfo)
            .Include(r => r.Comments)
            .ThenInclude(c => c.UserInfo)
            .Where(x => x.UserInfoId == userid)
            .OrderByDescending(x => x.DateCreate)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<DTORefractory>>(refractories);
    }

    public async Task<IEnumerable<DTORefractory>> GetRefractoriesByTypeAsync(string refType)
    {
        var context = _uow.Context;
        var refractories = await context.Refractories
            .Include(r => r.UserInfo)
            .Include(r => r.Comments)
            .ThenInclude(c => c.UserInfo)
            .Where(x => x.RefractoryType == refType)
            .OrderByDescending(x => x.DateCreate)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<DTORefractory>>(refractories);
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