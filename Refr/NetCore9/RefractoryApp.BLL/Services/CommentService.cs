using AutoMapper;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.BLL.Services;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public void Dispose()
    {
        _uow?.Dispose();
    }

    public async Task AddCommentAsync(DTOComment dtoComment)
    {
        if (dtoComment.UserInfoId <= 0)
            throw new ArgumentException("UserInfoId must be greater than 0", nameof(dtoComment.UserInfoId));
        
        if (string.IsNullOrWhiteSpace(dtoComment.CommentBody))
            throw new ArgumentException("Comment body cannot be empty", nameof(dtoComment.CommentBody));

        var comment = _mapper.Map<Comment>(dtoComment);
        comment.DateCreation = DateTime.Now;
        
        await _uow.Comments.InsertAsync(comment);
        await _uow.SaveAsync();
    }

    public async Task DeleteCommentAsync(int dtoCommentId)
    {
        await _uow.Comments.DeleteAsync(dtoCommentId);
        await _uow.SaveAsync();
    }

    public async Task<IEnumerable<DTOComment>> GetCommentsForRefractoryAsync(int refId)
    {
        var comments = await _uow.Comments.SelectAllAsync(x => x.RefractoryId == refId);
        var sortedComments = comments.OrderByDescending(x => x.DateCreation);
        
        return _mapper.Map<IEnumerable<DTOComment>>(sortedComments);
    }

    public async Task<DTOComment?> GetCommentByIdAsync(int comId)
    {
        var comment = await _uow.Comments.SelectByIdAsync(comId);
        return comment != null ? _mapper.Map<DTOComment>(comment) : null;
    }
}