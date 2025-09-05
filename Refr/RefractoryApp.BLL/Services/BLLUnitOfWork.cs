using RefractoryApp.BLL.Contracts;

namespace RefractoryApp.BLL.Services;

public class BLLUnitOfWork : IBLLUnitOfWork
{
    public IAdminService AdminService { get; }
    public ICommentService CommentService { get; }
    public IRefractoryService RefractoryService { get; }
    public IUserInfoService UserInfoService { get; }
    public IUserManagerService UserManagerService { get; }
    public IModeratorService ModeratorService { get; }

    public BLLUnitOfWork(
        IAdminService adminService,
        ICommentService commentService,
        IRefractoryService refractoryService,
        IUserInfoService userInfoService,
        IUserManagerService userManagerService,
        IModeratorService moderatorService)
    {
        AdminService = adminService;
        CommentService = commentService;
        RefractoryService = refractoryService;
        UserInfoService = userInfoService;
        UserManagerService = userManagerService;
        ModeratorService = moderatorService;
    }

    public void Dispose()
    {
        AdminService?.Dispose();
        CommentService?.Dispose();
        RefractoryService?.Dispose();
        UserInfoService?.Dispose();
        ModeratorService?.Dispose();
    }
}