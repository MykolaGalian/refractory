using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.DAL.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Roles
        await SeedRolesAsync(roleManager);
        
        // Seed Users
        await SeedUsersAsync(userManager, context);
        
        // Seed Sample Data
        await SeedSampleDataAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = { "user", "moderator", "admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }
        }
    }

    private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        // Admin user
        var adminUser = await userManager.FindByNameAsync("Myko");
        if (adminUser == null)
        {
            var admin = new ApplicationUser
            {
                Email = "Myko@gmail.com",
                UserName = "Myko"
            };
            
            var result = await userManager.CreateAsync(admin, "admin1");
            Console.WriteLine($"Creating admin user Myko: {(result.Succeeded ? "SUCCESS" : "FAILED")}");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                }
            }
                
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
                await userManager.AddToRoleAsync(admin, "moderator");
                
                var userInfo = new UserInfo
                {
                    Id = admin.Id,
                    Name = "Микола",
                    LastName = "Петрович", 
                    Login = "Myko",
                    UserAvatar = "mmm.png",
                    Position = "Зам. начальника АСУТП",
                    IsBlocked = false,
                    DateRegistration = DateTime.Now
                };
                
                context.UserInfos.Add(userInfo);
                await context.SaveChangesAsync();
            }
        }
        else if (adminUser != null && !await context.UserInfos.AnyAsync(u => u.Id == adminUser.Id))
        {
            var userInfo = new UserInfo
            {
                Id = adminUser.Id,
                Name = "Микола",
                LastName = "Петрович", 
                Login = "Myko",
                UserAvatar = "mmm.png",
                Position = "Зам. начальника АСУТП",
                IsBlocked = false,
                DateRegistration = DateTime.Now
            };
            
            context.UserInfos.Add(userInfo);
            await context.SaveChangesAsync();
        }

        // Moderator user
        var moderUser = await userManager.FindByNameAsync("Kostya");
        if (moderUser == null)
        {
            var moder = new ApplicationUser
            {
                Email = "Kostya@gmail.com",
                UserName = "Kostya"
            };

            var result = await userManager.CreateAsync(moder, "moder1");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(moder, "moderator");
                
                var moderInfo = new UserInfo
                {
                    Id = moder.Id,
                    Name = "Костянтин",
                    LastName = "Левко",
                    Login = "Kostya", 
                    UserAvatar = "kkk.png",
                    Position = "Зам. начальника ТO",
                    IsBlocked = false,
                    DateRegistration = DateTime.Now
                };
                
                context.UserInfos.Add(moderInfo);
                await context.SaveChangesAsync();
            }
        }
        else if (moderUser != null && !await context.UserInfos.AnyAsync(u => u.Id == moderUser.Id))
        {
            var moderInfo = new UserInfo
            {
                Id = moderUser.Id,
                Name = "Костянтин",
                LastName = "Левко",
                Login = "Kostya", 
                UserAvatar = "kkk.png",
                Position = "Зам. начальника ТO",
                IsBlocked = false,
                DateRegistration = DateTime.Now
            };
            
            context.UserInfos.Add(moderInfo);
            await context.SaveChangesAsync();
        }

        // Regular users
        var dymaUser = await userManager.FindByNameAsync("Dyma");
        if (dymaUser == null)
        {
            var user1 = new ApplicationUser
            {
                Email = "Dyma@gmail.com",
                UserName = "Dyma"
            };

            var result = await userManager.CreateAsync(user1, "user11");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user1, "user");
                
                var user1Info = new UserInfo
                {
                    Id = user1.Id,
                    Name = "Дмитро",
                    LastName = "Дорн",
                    Login = "Dyma",
                    UserAvatar = "jjj.png", 
                    Position = "Майстер розливки цеху №2",
                    IsBlocked = false,
                    DateRegistration = DateTime.Now
                };
                
                context.UserInfos.Add(user1Info);
                await context.SaveChangesAsync();
            }
        }
        else if (dymaUser != null && !await context.UserInfos.AnyAsync(u => u.Id == dymaUser.Id))
        {
            var user1Info = new UserInfo
            {
                Id = dymaUser.Id,
                Name = "Дмитро",
                LastName = "Дорн",
                Login = "Dyma",
                UserAvatar = "jjj.png", 
                Position = "Майстер розливки цеху №2",
                IsBlocked = false,
                DateRegistration = DateTime.Now
            };
            
            context.UserInfos.Add(user1Info);
            await context.SaveChangesAsync();
        }

        var stepUser = await userManager.FindByNameAsync("Step");
        if (stepUser == null)
        {
            var user2 = new ApplicationUser
            {
                Email = "Step@gmail.com",
                UserName = "Step"
            };

            var result = await userManager.CreateAsync(user2, "user21");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, "user");
                
                var user2Info = new UserInfo
                {
                    Id = user2.Id,
                    Name = "Степан",
                    LastName = "Лом",
                    Login = "Step",
                    UserAvatar = "sss.png",
                    Position = "Майстер розливки цеху №3",
                    IsBlocked = false,
                    DateRegistration = DateTime.Now
                };
                
                context.UserInfos.Add(user2Info);
                await context.SaveChangesAsync();
            }
        }
        else if (stepUser != null && !await context.UserInfos.AnyAsync(u => u.Id == stepUser.Id))
        {
            var user2Info = new UserInfo
            {
                Id = stepUser.Id,
                Name = "Степан",
                LastName = "Лом",
                Login = "Step",
                UserAvatar = "sss.png",
                Position = "Майстер розливки цеху №3",
                IsBlocked = false,
                DateRegistration = DateTime.Now
            };
            
            context.UserInfos.Add(user2Info);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Check if sample data already exists
        if (await context.Refractories.AnyAsync())
            return;

        // Get the user to associate with the refractory
        var user = await context.UserInfos.FirstOrDefaultAsync(u => u.Login == "Dyma");
        if (user == null) return;

        var refractory = new Refractory
        {
            RefractoryBrand = "RefraTech 345P",
            RefractoryDescription = "Периклазо-вуглецеий кирпич для робочої стіни сталь ковша. Має високу стійкість, що досягає 50 і більше плавок.",
            UserInfoId = user.Id,
            RefractoryPicture = "111.png",
            DateCreate = DateTime.Now,
            RefractoryType = "#Робоча стiна",
            Density = 2.3f,
            MaxWorkTemperature = 1650.0f,
            Lime = 1.0f,
            Alumina = 0.1f,
            Silica = 1.1f,
            Magnesia = 77.5f,
            Carbon = 15.0f,
            Price = 1200.5f
        };

        context.Refractories.Add(refractory);
        await context.SaveChangesAsync();

        // Add sample comments
        var stepUser = await context.UserInfos.FirstOrDefaultAsync(u => u.Login == "Step");
        
        var comments = new[]
        {
            new Comment
            {
                CommentBody = "Кирпич добре себе показав у ході компанії від 20.01.2019 на броні №22. Стійкість склала 55 плавок",
                UserInfoId = user.Id,
                DateCreation = DateTime.Now,
                RefractoryId = refractory.Id
            },
            new Comment
            {
                CommentBody = "25.01.2019 на броні №23 стійкість склала 50 плавок",
                UserInfoId = stepUser?.Id ?? user.Id,
                DateCreation = DateTime.Now,
                RefractoryId = refractory.Id
            }
        };

        context.Comments.AddRange(comments);
        await context.SaveChangesAsync();
    }
}