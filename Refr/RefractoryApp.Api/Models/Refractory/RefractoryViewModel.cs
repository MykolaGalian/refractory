using RefractoryApp.Api.Models.Comment;
using RefractoryApp.Api.Models.User;

namespace RefractoryApp.Api.Models.Refractory;

public class RefractoryViewModel
{
    public int Id { get; set; }
    public string RefractoryDescription { get; set; } = string.Empty;
    public string RefractoryBrand { get; set; } = string.Empty;
    public string RefractoryPicture { get; set; } = string.Empty;
    public int UserInfoId { get; set; }
    public virtual ProfileViewModel? UserInfo { get; set; }
    public DateTime? DateCreate { get; set; }
    public DateTime? LastEdit { get; set; }
    public ICollection<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    public string RefractoryType { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public float Density { get; set; }
    public float MaxWorkTemperature { get; set; }
    public float Lime { get; set; }
    public float Alumina { get; set; }
    public float Silica { get; set; }
    public float Magnesia { get; set; }
    public float Carbon { get; set; }
    public float Price { get; set; }
}