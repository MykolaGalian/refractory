using System.ComponentModel.DataAnnotations;

namespace RefractoryApp.Api.Models.Refractory;

public class RefractoryEditViewModel
{
    public string RefractoryDescription { get; set; } = string.Empty;
    public string RefractoryBrand { get; set; } = string.Empty;

    [Display(Name = "RefractoryType:")]
    [MaxLength(50)]
    public string RefractoryType { get; set; } = string.Empty;

    public float Density { get; set; }
    public float MaxWorkTemperature { get; set; }
    public float Lime { get; set; }
    public float Alumina { get; set; }
    public float Silica { get; set; }
    public float Magnesia { get; set; }
    public float Carbon { get; set; }
    public float Price { get; set; }
}