using System.ComponentModel.DataAnnotations;

namespace RefractoryApp.Api.Models.User;

public class ChangeProfileViewModel
{
    public int Id { get; set; }

    [Display(Name = "Login:")]
    public string Login { get; set; } = string.Empty;

    [Display(Name = "Name:")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Lastname:")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Position:")]
    public string Position { get; set; } = string.Empty;

    [Display(Name = "Avatar:")]
    public string UserAvatar { get; set; } = string.Empty;

    [Display(Name = "Email:")]
    public string Email { get; set; } = string.Empty;

    public DateTime? DateRegistration { get; set; }
    public bool IsBlocked { get; set; }
}