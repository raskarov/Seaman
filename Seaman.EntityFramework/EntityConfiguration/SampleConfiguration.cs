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

            Property(it => it.DepositorSsnType);
            Property(it => it.PartnerSsnType);

            Property(it => it.DepositorAddress).HasMaxLength(100);
            Property(it => it.DepositorCity).HasMaxLength(50);
            Property(it => it.DepositorState).HasMaxLength(20);
            Property(it => it.DepositorZip);
            Property(it => it.DepositorHomePhone).HasMaxLength(50);
            Property(it => it.DepositorCellPhone).HasMaxLength(50);
            Property(it => it.DepositorEmail).HasMaxLength(50);

            Property(it => it.PartnerAddress).HasMaxLength(100);
            Property(it => it.PartnerCity).HasMaxLength(50);
            Property(it => it.PartnerState).HasMaxLength(20);
            Property(it => it.PartnerZip);
            Property(it => it.PartnerHomePhone).HasMaxLength(50);
            Property(it => it.PartnerCellPhone).HasMaxLength(50);
            Property(it => it.PartnerEmail).HasMaxLength(50);
        }
    }
}
