using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Location : LocationBase
    {
        //public virtual Tank Tank { get; set; }
        //public virtual Canister Canister { get; set; }
        //public virtual Cane Cane { get; set; }
        public virtual Sample Sample { get; set; }
    }
}
