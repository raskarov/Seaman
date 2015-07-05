using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework.EntityConfiguration
{
    public class LocationConfig : EntityTypeConfiguration<Location>
    {
        public LocationConfig()
        {
            Property(it => it.UniqName).HasMaxLength(50);
            Property(it => it.Tank).HasMaxLength(10);
            Property(it => it.CaneColor).HasMaxLength(20);
            Property(it => it.SpecimenNumber).HasMaxLength(20);
        }
    }
}
