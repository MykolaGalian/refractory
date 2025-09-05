using RefractoryApp.BLL.DTO;

namespace RefractoryApp.BLL.Contracts;

public interface IRefractoryService : IDisposable
{
    Task<List<DTORefractory>> GetAllRefractoriesAsync();
    Task AddRefractoryAsync(DTORefractory dtoRefractory);
    Task EditRefractoryAsync(DTORefractory dtoRefractory);
    Task DeleteRefractoryAsync(int refId);
    Task<DTORefractory?> GetRefractoryByRefIdAsync(int refId);
    Task<IEnumerable<DTORefractory>> GetRefractoriesByUserIdAsync(int userid);
    
    Task<IEnumerable<DTORefractory>> GetRefractoriesByTypeAsync(string refType);
    Task<IEnumerable<string>> GetAllRefTypesAsync();
    
    Task<DTORefractory?> GetRefractoryByBrandAsync(string refBrand);
}