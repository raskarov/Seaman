using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Physician: PhysicianBase
    {
        public virtual ICollection<Sample> Samples
        {
            get { return _samples; }
            set { _samples = value; }
        }

        private ICollection<Sample> _samples = new HashSet<Sample>();
    }
}
