using System.ComponentModel.DataAnnotations;

namespace RefractoryApp.Api.Models.Account;

public class LoginViewModel
{
    [Required]
    [StringLength(10, MinimumLength = 3)]
    public string Login { get; set; } = string.Empty;

    [Required]
    [StringLength(10, MinimumLength = 3)]
    public string Password { get; set; } = string.Empty;
}