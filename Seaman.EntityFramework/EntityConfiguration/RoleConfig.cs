using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework.EntityConfiguration
{
    public class RoleConfig : EntityTypeConfiguration<Role>
    {
        public RoleConfig()
        {
            Property(it => it.Name).IsRequired().HasMaxLength(50).HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_Role_Name") { IsUnique = true }));
            Property(it => it.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
