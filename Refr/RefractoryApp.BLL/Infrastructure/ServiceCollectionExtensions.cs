using Microsoft.Extensions.DependencyInjection;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.Services;

namespace RefractoryApp.BLL.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBLLServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Register BLL services
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IRefractoryService, RefractoryService>();
        services.AddScoped<IUserInfoService, UserInfoService>();
        services.AddScoped<IUserManagerService, UserManagerService>();
        services.AddScoped<IModeratorService, ModeratorService>();

        // Register BLL Unit of Work
        services.AddScoped<IBLLUnitOfWork, BLLUnitOfWork>();

        return services;
    }
}