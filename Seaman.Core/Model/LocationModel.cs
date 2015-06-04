using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class LocationModel : LocationBase
    {
        public Int32 CaneId { get; set; }
        public Int32 TankId { get; set; }
        public Int32 CanisterId { get; set; }
        public Int32 SampleId { get; set; }
    }

    public class LocationBase : NamedBase
    {
        public Boolean Available { get; set; }
        public String UniqName { get; set; }
    }
}
