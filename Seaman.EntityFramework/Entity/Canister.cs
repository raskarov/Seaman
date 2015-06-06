using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Canister : CanisterBase
    {
        public virtual Tank Tank { get; set; }
        public virtual ICollection<Cane> Canes
        {
            get { return _canes; }
            set { _canes = value; }
        }

        private ICollection<Cane> _canes = new HashSet<Cane>();
    }
}
