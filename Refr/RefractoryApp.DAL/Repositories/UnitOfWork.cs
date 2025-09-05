using Microsoft.AspNetCore.Identity;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Data;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private IGenericRepository<Comment>? _comments;
    private IGenericRepository<Refractory>? _refractories;
    private IGenericRepository<UserInfo>? _userInfos;

    public UnitOfWork(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public UserManager<ApplicationUser> UserManager => _userManager;
    public RoleManager<ApplicationRole> RoleManager => _roleManager;
    public ApplicationDbContext Context => _context;

    public IGenericRepository<Comment> Comments =>
        _comments ??= new GenericRepository<Comment>(_context);

    public IGenericRepository<Refractory> Refractories =>
        _refractories ??= new GenericRepository<Refractory>(_context);

    public IGenericRepository<UserInfo> UserInfos =>
        _userInfos ??= new GenericRepository<UserInfo>(_context);

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}