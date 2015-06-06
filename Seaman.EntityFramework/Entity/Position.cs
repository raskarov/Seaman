using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Position: PositionBase
    {
        public virtual Cane Cane { get; set; }
        public virtual ICollection<Location> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private ICollection<Location> _locations = new HashSet<Location>();
    }
}
