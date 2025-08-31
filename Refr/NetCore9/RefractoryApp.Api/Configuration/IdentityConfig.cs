using Microsoft.AspNetCore.Identity;
using RefractoryApp.DAL.Data;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.Api.Configuration;

public static class IdentityConfig
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        // Настройка Identity с кастомными типами и int ключами
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Настройки пользователей (аналог UserValidator из старого проекта)
            options.User.AllowedUserNameCharacters = 
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            options.User.RequireUniqueEmail = true;

            // Настройки паролей (аналог PasswordValidator из старого проекта)
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;

            // Настройки блокировки
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // Настройки входа
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}

// Расширения для UserManager (аналог методов из старого ApplicationUserManager)
public static class UserManagerExtensions
{
    public static async Task<IdentityResult> CreateUserWithInfoAsync(
        this UserManager<ApplicationUser> userManager,
        string email,
        string userName,
        string password,
        UserInfo userInfo)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = userName,
            UserInfo = userInfo
        };

        return await userManager.CreateAsync(user, password);
    }
}