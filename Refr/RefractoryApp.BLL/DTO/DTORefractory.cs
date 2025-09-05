namespace RefractoryApp.BLL.DTO;

public class DTORefractory
{
    public int Id { get; set; }
    public string RefractoryDescription { get; set; } = string.Empty;
    public string RefractoryBrand { get; set; } = string.Empty;
    public string? RefractoryPicture { get; set; }
    public DateTime DateCreate { get; set; }
    public DateTime? LastEdit { get; set; }
    public string? RefractoryType { get; set; }
    public bool IsBlocked { get; set; }

    public float Density { get; set; }
    public float MaxWorkTemperature { get; set; }
    
    public float Lime { get; set; }
    public float Alumina { get; set; }
    public float Silica { get; set; }
    public float Magnesia { get; set; }
    public float Carbon { get; set; }
    public float Price { get; set; }

    public int UserInfoId { get; set; }
    public virtual DTOUser? UserInfo { get; set; }

    public virtual ICollection<DTOComment> Comments { get; set; } = new List<DTOComment>();
}