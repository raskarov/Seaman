using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seaman.Core
{
    public class UserModel : UserProfileBase
    {
        
    }

    public class UserBase : IEntity
    {
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? LockedUntil { get; set; }
        public String Password { get; set; }
        public String PasswordType { get; set; }
    }

    public class UserProfileBase : UserBase
    {
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
    }
}
