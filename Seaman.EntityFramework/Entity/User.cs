using System.Collections.Generic;
using Seaman.Core;

namespace Seaman.EntityFramework.Entity
{
    public class User : UserProfileBase
    {


        public virtual ICollection<Role> Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }

        public virtual ICollection<Sample> Samples
        {
            get { return _samples; }
            set { _samples = value; }
        }
        
        #region Private

        private ICollection<Role> _roles = new HashSet<Role>();
        private ICollection<Sample> _samples = new HashSet<Sample>(); 
        #endregion
    }
}
