using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.Api.Models.Comment;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IBLLUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CommentController(IBLLUnitOfWork uow, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _uow = uow;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpPost("{refId}")]
    public async Task<IActionResult> AddComment(int refId, [FromForm] string comment)
    {
        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null)
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(comment))
        {
            ModelState.AddModelError("comment", "Please write comment.");
            return BadRequest(ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _uow.UserInfoService.GetUserByIdAsync(int.Parse(currentUserId));
        if (user == null)
            return Unauthorized();
            
        var dtoComment = new DTOComment 
        { 
            DateCreation = DateTime.Now, 
            RefractoryId = refId, 
            UserInfoId = user.Id, 
            CommentBody = comment 
        };

        await _uow.CommentService.AddCommentAsync(dtoComment);
        return Ok("Comment added.");
    }

    [HttpGet("{refId}")]
    public async Task<IActionResult> GetComments(int refId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        if (refractory.IsBlocked)
            return BadRequest("This refractory blocked");

        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null)
            return Unauthorized();

        var comments = await _uow.CommentService.GetCommentsForRefractoryAsync(refId);
        var commentViewModels = _mapper.Map<List<CommentViewModel>>(comments);

        return Ok(commentViewModels);
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> Delete(int commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentUserName = _userManager.GetUserName(User);
        if (currentUserName == null)
            return Unauthorized();

        var userIdString = _userManager.GetUserId(User);
        if (userIdString == null || !int.TryParse(userIdString, out var currentUserId))
            return Unauthorized();

        var comment = await _uow.CommentService.GetCommentByIdAsync(commentId);
        if (comment == null)
            return NotFound();

        var post = await _uow.RefractoryService.GetRefractoryByRefIdAsync(comment.RefractoryId);
        if (post == null)
            return NotFound();

        if (comment.UserInfoId != currentUserId)
            return BadRequest("It is not your comment");

        await _uow.CommentService.DeleteCommentAsync(commentId);
        return Ok("Comment deleted");
    }
}