using Microsoft.AspNetCore.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.DAL.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public virtual UserInfo? UserInfo { get; set; }
}