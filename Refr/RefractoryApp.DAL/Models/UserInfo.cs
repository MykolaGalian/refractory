using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.DAL.Models;

[Table("UserInfo")]
public class UserInfo : BaseEntity
{
    [MaxLength(30)]
    public string? Name { get; set; }
    
    [MaxLength(30)]
    public string? LastName { get; set; }
    
    [MaxLength(30)]
    [MinLength(3)]
    public string Login { get; set; } = string.Empty;
    
    public string? UserAvatar { get; set; }
    
    [MaxLength(100)]
    public string? Position { get; set; }
    
    public bool IsBlocked { get; set; }
    
    public DateTime? DateRegistration { get; set; }

    // В старом проекте Id UserInfo совпадает с Id ApplicationUser
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override int Id { get; set; }
    
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;

    public virtual ICollection<Refractory> Refractories { get; set; } = new List<Refractory>();
}