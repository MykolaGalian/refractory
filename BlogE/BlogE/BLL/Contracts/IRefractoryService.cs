using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface IRefractoryService : IDisposable
    {
        Task<List<DTORefractory>> GetAllRefractories();        
        Task AddRefractory(DTORefractory dtoRefractory);
        Task EditRefractory(DTORefractory dtoRefractory);
        Task DeleteRefractory(int refId);
        Task<DTORefractory> GetRefractoryByRefId(int refId);
        Task<IEnumerable<DTORefractory>> GetRefractoriesByUserId(int userid);
        
        Task<IEnumerable<DTORefractory>> GetRefractoriesByType(string refType);
        Task<IEnumerable<string>> GetAllRefTypes();  //tags

        Task<DTORefractory> GetRefractoryByBrand(string refBrand);


    }
}
