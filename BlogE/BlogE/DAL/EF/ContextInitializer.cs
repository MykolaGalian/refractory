using DAL.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using DAL.Models;

namespace DAL.EF
{
    public class ContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            var userManager = new ApplicationUserManager(new CustomUserStore(db));

            var roleManager = new ApplicationRoleManager(new CustomRoleStore(db));

            var role1 = new CustomRole("user");
            var role2 = new CustomRole("moderator");
            var role3 = new CustomRole("admin");

            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            var admin = new ApplicationUser()
            {
                Email = "Myko@gmail.com",
                UserName = "Myko",
                UserInfo = new UserInfo() { Name = "Mикола", LastName = "Петрович", Login = "Myko", UserAvatar = "mmm.png", Position = "Зам. начальника АСУТП",  IsBlocked = false,  DateRegistration = DateTime.Now }
            };
            var result = userManager.Create(admin, "111111");

            var moder  = new ApplicationUser()
            {
                Email = "Kostya@gmail.com",
                UserName = "Kostya",
                UserInfo = new UserInfo() { Name = "Костянтин", LastName = "Левко", Login = "Kostya", UserAvatar = "kkk.png", Position = "Зам. начальника ТO", IsBlocked = false, DateRegistration = DateTime.Now }
            };
            var result2 = userManager.Create(moder, "222222"); // min 6 characters

            var user1 = new ApplicationUser()
            {
                Email = "Dyma@gmail.com",
                UserName = "Dyma",
                UserInfo = new UserInfo() { Name = "Дмитро", LastName = "Дорн", Login = "Dyma", UserAvatar = "jjj.png", Position = "Майстер розливки цеху №2", IsBlocked = false, DateRegistration = DateTime.Now }
            };
            var result3 = userManager.Create(user1, "333333");

            var user2 = new ApplicationUser()
            {
                Email = "Step@gmail.com",
                UserName = "Step",
                UserInfo = new UserInfo() { Name = "Степан", LastName = "Лом", Login = "Step", UserAvatar = "sss.png", Position = "Майстер розливки цеху №3", IsBlocked = false, DateRegistration = DateTime.Now }
            };
            var result4 = userManager.Create(user2, "444444");
            db.SaveChanges();

            var ref1 = new Refractory()
            {
                RefractoryBrand = "RefraTech 345P",
                RefractoryDescription = "Периклазо-вуглецеий кирпич для робочої стіни сталь ковша. Має високу стійкість, що досягає 50 і більше плавок.",
                UserInfoId = 3,
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
            db.Refractory_.Add(ref1);
            db.SaveChanges();

            var comment = new Comment() { CommentBody = "Кирпич добре себе показав у ході компанії від 20.01.2019 на броні №22. Стійкість склала 55 плавок", UserInfoId = 3, DateCreation = DateTime.Now, RefractoryId = 1 };
            var comment2 = new Comment() { CommentBody = "25.01.2019 на броні №23 стійкість склала 50 плавок", UserInfoId = 4, DateCreation = DateTime.Now, RefractoryId = 1 };

            db.Comment_.Add(comment);
            db.Comment_.Add(comment2);
            db.SaveChanges();
            
            if (result.Succeeded && result2.Succeeded && result3.Succeeded && result4.Succeeded)
            {
                userManager.AddToRole(admin.Id, role3.Name);
                userManager.AddToRole(admin.Id, role2.Name);
                userManager.AddToRole(moder.Id, role2.Name);
                userManager.AddToRole(user1.Id, role1.Name);
                userManager.AddToRole(user2.Id, role1.Name);
            }
            db.SaveChanges();
        }

    }
}
