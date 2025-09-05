using Microsoft.AspNetCore.Identity;
using RefractoryApp.DAL.Data;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.DAL.Contracts;

public interface IUnitOfWork : IDisposable
{
    UserManager<ApplicationUser> UserManager { get; }
    RoleManager<ApplicationRole> RoleManager { get; }
    ApplicationDbContext Context { get; }

    IGenericRepository<Comment> Comments { get; }
    IGenericRepository<Refractory> Refractories { get; }
    IGenericRepository<UserInfo> UserInfos { get; }
    
    Task SaveAsync();
}