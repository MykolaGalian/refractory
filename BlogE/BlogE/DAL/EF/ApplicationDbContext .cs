using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Identity;
using DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin,
        CustomUserRole, CustomUserClaim>
    {
        static ApplicationDbContext()  {            

          //  Database.SetInitializer<ApplicationDbContext>(new ContextInitializer()); //remove after creation DB
        }

        public ApplicationDbContext(string connection) : base(connection)
        {
           // Database.Initialize(true);   //remove after creation DB
        }

        public ApplicationDbContext() : base("RefractoryDB")
        {

        }

        public DbSet<Comment> Comment_ { get; set; }  
        public DbSet<Refractory> Refractory_ { get; set; }
        public DbSet<UserInfo> UserInfo_ { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {           
            //to prevent cycles or multiple cascade paths
            modelBuilder.Entity<Refractory>().HasRequired(i => i.UserInfo).WithMany().WillCascadeOnDelete(false);
           

            base.OnModelCreating(modelBuilder);
        }

    }
    //https://docs.microsoft.com/en-us/aspnet/identity/overview/extensibility/change-primary-key-for-users-in-aspnet-identity
}
