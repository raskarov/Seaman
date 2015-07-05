using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core;

namespace Seaman.EntityFramework.Entity
{
    public class Sample : SampleBase
    {
        public virtual Physician Physician { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ModifiedByUser { get; set; }

        public virtual ICollection<Location> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        private ICollection<Location> _locations = new HashSet<Location>();
    }
}
