using Seaman.Core;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SeamanDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(SeamanDbContext context)
        {
            ServiceMappers.Check();
            EnsureRole(context, new Role { Id = (Int32)Roles.Admin, Name = "Admin" });
            EnsureRole(context, new Role { Id = (Int32)Roles.Embryologist, Name = "Embryologist" });
            EnsureRole(context, new Role { Id = (Int32)Roles.ReportGenerator, Name = "ReportGenerator" });

            var usermanager = new UserManager(context, str => new PlainHasher());
            var user = usermanager.FindByName("admin");
            if (user == null)
            {
                user = usermanager.Add(new UserModel { UserName = "admin" });
                usermanager.SetUserPassword(user.Id, "111111", "plain");
                usermanager.AddRoleToUser(user.Id, (Int32)Roles.Admin);
            }

            context.SaveChanges();
        }

        public static void EnsureRole(SeamanDbContext context, Role role)
        {
            var dbrole = context.Roles.Find(role.Id);
            if (dbrole == null)
                context.Roles.Add(role);
        }
    }
}
