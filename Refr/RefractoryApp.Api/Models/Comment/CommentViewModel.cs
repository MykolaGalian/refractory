using RefractoryApp.Api.Models.User;

namespace RefractoryApp.Api.Models.Comment;

public class CommentViewModel
{
    public int Id { get; set; }
    public string CommentBody { get; set; } = string.Empty;
    public int RefractoryId { get; set; }
    public DateTime? DateCreation { get; set; }
    public DateTime? LastEdit { get; set; }
    public int? UserInfoId { get; set; }
    public ProfileViewModel? UserInfo { get; set; }
}