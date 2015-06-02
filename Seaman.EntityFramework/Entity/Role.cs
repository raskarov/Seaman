using System.Collections.Generic;
using Seaman.Core;

namespace Seaman.EntityFramework.Entity
{
    public class Role : RoleBase
    {
        private ICollection<User> _users = new HashSet<User>();


        public virtual ICollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }
    }
}