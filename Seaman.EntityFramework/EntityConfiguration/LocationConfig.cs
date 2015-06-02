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
        }
    }
}
