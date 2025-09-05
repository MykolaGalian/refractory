using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefractoryApp.DAL.Models;

[Table("Comment")]
public class Comment : BaseEntity
{
    [MaxLength(255)]
    public string CommentBody { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateCreation { get; set; }
    
    public DateTime? DateEdit { get; set; }

    [ForeignKey("UserInfoId")]
    public virtual UserInfo UserInfo { get; set; } = null!;
    
    public int UserInfoId { get; set; }

    [ForeignKey("RefractoryId")]
    public virtual Refractory Refractory { get; set; } = null!;
    
    public int RefractoryId { get; set; }
}