using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public interface ICommentService : IDisposable
    {
        Task AddComment(DTOComment dtoComment);
        Task DeleteComment(int dtoCommentId); 
        Task<IEnumerable<DTOComment>> GetCommentsForRefractory(int refId);
        Task<DTOComment> GetCommentById(int comId);
    }
}
