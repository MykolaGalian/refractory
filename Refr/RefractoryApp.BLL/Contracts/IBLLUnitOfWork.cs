namespace RefractoryApp.BLL.Contracts;

public interface IBLLUnitOfWork : IDisposable
{
    IAdminService AdminService { get; }
    ICommentService CommentService { get; }
    IRefractoryService RefractoryService { get; }
    IUserInfoService UserInfoService { get; }
    IUserManagerService UserManagerService { get; }
    IModeratorService ModeratorService { get; }
}