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
        public Boolean Extracted { get; set; }
        public String Tank { get; set; }
        public Int32 Canister { get; set; }
        public String Cane { get; set; }
        public Int32 Position { get; set; }

        public String UniqName { get; set; }
        public DateTime? DateStored { get; set; }
        public Int32? CollectionMethodId { get; set; }
    }

    public class LocationBriefModel
    {
        public Int32 Id { get; set; }
        public String UniqName { get; set; }
        public String DateStored { get; set; }
        public Int32? CollectionMethodId { get; set; }
        public String CollectionMethod { get; set; }
    }
}
