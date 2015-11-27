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
            AutomaticMigrationsEnabled = true;

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

            var embryologist = usermanager.FindByName("embryologist");
            if (embryologist == null)
            {
                embryologist = usermanager.Add(new UserModel { UserName = "embryologist" });
                usermanager.SetUserPassword(embryologist.Id, "111111", "plain");
                usermanager.AddRoleToUser(embryologist.Id, (Int32)Roles.Embryologist);
            }
            
            context.SaveChanges();
            
            

            //if (!context.Tanks.Any())
            //{
            //    var colors = new String[] { "white", "blue", "green", "black", "red", "yellow", "purple", "orange" };
            //    const string alpabeth = "abcdefghijklmnopqrstuvwxyz";
            //    var letters = alpabeth.Substring(0, alpabeth.Length - 1).ToUpper().ToCharArray();

            //    var tank = context.CreateAndAdd<Tank>();
            //    tank.Name = "A";
            //    context.SaveChanges();

            //    if (!context.Canisters.Any(x => x.Tank.Id == tank.Id))
            //    {
            //        for (var i = 0; i < 6; i++)
            //        {
            //            var canister = new Canister()
            //            {
            //                Name = (i + 1).ToString()
            //            };
            //            tank.Canisters.Add(canister);
            //            context.SaveChanges();
            //            if (!context.Canes.Any(x => x.Canister.Id == canister.Id))
            //            {
            //                foreach (var letter in letters)
            //                {
            //                    foreach (var color in colors)
            //                    {
            //                        var cane = new Cane()
            //                        {
            //                            Name = new String(letter, 1),
            //                            Color = color
            //                        };

            //                        canister.Canes.Add(cane);
            //                        context.SaveChanges();

            //                        if (!context.Positions.Any(x => x.Cane.Id == cane.Id))
            //                        {
            //                            for (var j = 0; j < 6; j++)
            //                            {
            //                                var position = new Position()
            //                                {
            //                                    Name = (j + 1).ToString()
            //                                };

            //                                cane.Positions.Add(position);
            //                                context.SaveChanges();
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public static void EnsureRole(SeamanDbContext context, Role role)
        {
            var dbrole = context.Roles.Find(role.Id);
            if (dbrole == null)
                context.Roles.Add(role);
        }
    }
}
