using System;

namespace Seaman.Core
{
    /// <summary>
    /// Plain text password hasher
    /// </summary>
    public class PlainHasher : IPasswordHasher
    {
        public String HashPassword(String password)
        {
            return password;
        }

        public Boolean VerifyHashedPassword(String hashedPassword, String password)
        {
            return hashedPassword == password;
        }
    }

}