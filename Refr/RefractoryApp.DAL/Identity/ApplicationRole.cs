using Microsoft.AspNetCore.Identity;

namespace RefractoryApp.DAL.Identity;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() { }
    
    public ApplicationRole(string name) : base(name) { }
}