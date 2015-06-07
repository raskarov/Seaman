using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class LocationModel : LocationBase
    {
        public Int32 SampleId { get; set; }
    }

    public class LocationBase : NamedBase
    {
        public Boolean Available { get; set; }
        public String Tank { get; set; }
        public Int32 Canister { get; set; }
        public String Cane { get; set; }
        public Int32 Position { get; set; }

        public String UniqName { get; set; }
    }
}
