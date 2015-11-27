using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class CryobankModel : CryobankBase
    {
    }

    public class CryobankBase : NamedBase
    {
        public string VialId { get; set; }
    }
}
