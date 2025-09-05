using System.ComponentModel.DataAnnotations;

namespace RefractoryApp.DAL.Models;

public abstract class BaseEntity
{
    [Key]
    public virtual int Id { get; set; }
}