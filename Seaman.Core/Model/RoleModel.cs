using System;

namespace Seaman.Core
{

    public class RoleBase : IEntity
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }

    public class RoleModel : RoleBase
    {
        
    }
}