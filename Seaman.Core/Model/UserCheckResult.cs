using System;

namespace Seaman.Core
{
    public class UserCheckResult
    {
        #region Public

        public Int32 UserId { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? LockedUntil { get; set; }
        public Boolean IsLocked
        {
            get
            {
                return LockedUntil.HasValue && LockedUntil > (_utc ?? (_utc = DateTime.UtcNow));
            }
        }
        public Boolean IsNotFound { get; set; }
        public Boolean IsWrongPassword { get; set; }
        public Boolean Success
        {
            get { return !IsNotFound && !IsDeleted && !IsLocked && !IsWrongPassword; }
        }


        #endregion
        
        #region Private

        private DateTime? _utc; 

        #endregion
    }
}