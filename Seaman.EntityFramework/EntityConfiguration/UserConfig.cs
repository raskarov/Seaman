using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework.EntityConfiguration
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            HasMany(it => it.Roles).WithMany(it => it.Users);

            Property(it => it.UserName).IsRequired().HasMaxLength(50).HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_User_UserName") { }));

            Property(it => it.PasswordType).HasMaxLength(50);
            Property(it => it.Phone).HasMaxLength(50);
            Property(it => it.Email).HasMaxLength(100);
            Property(it => it.FirstName).HasMaxLength(100);
            Property(it => it.MiddleName).HasMaxLength(100);
            Property(it => it.LastName).HasMaxLength(100);
        }
    }
}
