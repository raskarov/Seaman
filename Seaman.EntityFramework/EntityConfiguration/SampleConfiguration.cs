using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework.EntityConfiguration
{
    public class SampleConfiguration : EntityTypeConfiguration<Sample>
    {
        public SampleConfiguration()
        {
            Property(it => it.CryobankName).HasMaxLength(100);
            Property(it => it.CryobankVialId).HasMaxLength(20);
            
            Property(it => it.DepositorFirstName).HasMaxLength(50);
            Property(it => it.DepositorLastName).HasMaxLength(50);
            Property(it => it.DepositorSsn).HasMaxLength(20);
            
            Property(it => it.PartnerFirstName).HasMaxLength(50);
            Property(it => it.PartnerLastName).HasMaxLength(50);
            Property(it => it.PartnerSsn).HasMaxLength(20);

            Property(it => it.DirectedDonorId).HasMaxLength(20);
            Property(it => it.DirectedDonorFirstName).HasMaxLength(50);
            Property(it => it.DirectedDonorLastName).HasMaxLength(50);

            Property(it => it.AnonymousDonorId).HasMaxLength(20);

            Property(it => it.Comment).HasMaxLength(1000);
        }
    }
}
