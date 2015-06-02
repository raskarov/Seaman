using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core.Model
{
    public class NamedBase: IEntity
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }
    }
}
