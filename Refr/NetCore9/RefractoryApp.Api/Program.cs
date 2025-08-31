using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RefractoryApp.Api.Configuration;
using RefractoryApp.Api.Infrastructure;
using RefractoryApp.Api.Services;
using RefractoryApp.BLL.Infrastructure;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Data;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add OpenAPI/Swagger
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "RefractoryApp API", 
        Version = "v1" 
    });
});

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Custom Identity
builder.Services.AddCustomIdentity();

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourVerySecureSecretKeyHere123456789";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "RefractoryApp";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "RefractoryApp";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add JWT Service
builder.Services.AddScoped<JwtService>();

// Add DAL services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add BLL services
builder.Services.AddBLLServices();

// Add API AutoMapper profile
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));

// Add CORS for Angular client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

var app = builder.Build();

// Apply database migrations and seed data only if --migrate flag is provided
if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        Console.WriteLine("Applying database migrations...");
        await context.Database.MigrateAsync();

        Console.WriteLine("Seeding initial data...");
        await DataSeeder.SeedAsync(context, userManager, roleManager);
        
        Console.WriteLine("Database initialization completed successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization");
        throw; // Re-throw to prevent app from starting with broken DB
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "RefractoryApp API V1");
        options.SwaggerEndpoint("/openapi/v1.json", "RefractoryApp API V1 (OpenAPI)");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Enable serving static files from wwwroot
app.UseStaticFiles();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
