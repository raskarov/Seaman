using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class LocationModel : LocationBase
    {
    }

    public class LocationBase : NamedBase
    {
        public Boolean Available { get; set; }
        public String UniqName { get; set; }
    }
}
