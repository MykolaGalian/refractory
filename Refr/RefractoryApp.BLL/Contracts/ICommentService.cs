using RefractoryApp.BLL.DTO;

namespace RefractoryApp.BLL.Contracts;

public interface ICommentService : IDisposable
{
    Task AddCommentAsync(DTOComment dtoComment);
    Task DeleteCommentAsync(int dtoCommentId);
    Task<IEnumerable<DTOComment>> GetCommentsForRefractoryAsync(int refId);
    Task<DTOComment?> GetCommentByIdAsync(int comId);
}