using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class CaneModel : CaneBase
    {
    }

    public class CaneBase : NamedBase
    {
        public String Color { get; set; }
    }
}
