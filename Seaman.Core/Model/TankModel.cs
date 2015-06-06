using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class TankModel: TankBase
    {
    }

    public class TankBase: NamedBase
    {
        public Int32 CanistersCount { get; set; }
        public Int32 CanesCount { get; set; }
        public Int32 PositionsCount { get; set; }
    }
}
