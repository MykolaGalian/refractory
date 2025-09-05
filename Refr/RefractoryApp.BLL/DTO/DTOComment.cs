namespace RefractoryApp.BLL.DTO;

public class DTOComment
{
    public int Id { get; set; }
    public string CommentBody { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
    public DateTime? DateEdit { get; set; }

    public int UserInfoId { get; set; }
    public virtual DTOUser? UserInfo { get; set; }

    public int RefractoryId { get; set; }
    public virtual DTORefractory? Refractory { get; set; }
}