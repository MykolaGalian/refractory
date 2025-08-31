using System.ComponentModel.DataAnnotations;

namespace RefractoryApp.Api.Models.Account;

public class RegisterViewModel
{
    [Required]
    [MaxLength(10)]
    [MinLength(3)]
    [Display(Name = "Login:")]
    public string Login { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [MinLength(3)]
    [DataType(DataType.Password)]
    [Display(Name = "Password:")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password", ErrorMessage = "Password is not equal to the above")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password:")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [Display(Name = "Name:")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(10)]
    [Display(Name = "Lastname:")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email:")]
    [MaxLength(20)]
    [MinLength(5)]
    public string Email { get; set; } = string.Empty;
}