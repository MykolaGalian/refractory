using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        var context = _uow.Context;
        var comments = await context.Comments
            .Include(c => c.UserInfo)
            .Where(x => x.RefractoryId == refId)
            .OrderByDescending(x => x.DateCreation)
            .ToListAsync();
        
        return _mapper.Map<IEnumerable<DTOComment>>(comments);
    }

    public async Task<DTOComment?> GetCommentByIdAsync(int comId)
    {
        var context = _uow.Context;
        var comment = await context.Comments
            .Include(c => c.UserInfo)
            .FirstOrDefaultAsync(c => c.Id == comId);
            
        return comment != null ? _mapper.Map<DTOComment>(comment) : null;
    }
}