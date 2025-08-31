using RefractoryApp.DAL.Models;

namespace RefractoryApp.BLL.DTO;

public class DTOUser
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string? UserAvatar { get; set; }
    public string? Position { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime? DateRegistration { get; set; }

    public virtual ICollection<Refractory> Refractories { get; set; } = new List<Refractory>();
    public List<string> Roles { get; set; } = new List<string>();
}