using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core.Model;

namespace Seaman.EntityFramework.Entity
{
    public class Tank : TankBase
    {
        public virtual ICollection<Canister> Canisters
        {
            get { return _canisters; }
            set { _canisters = value; }
        }

        private ICollection<Canister> _canisters = new HashSet<Canister>();
    }
}
