using System;

namespace Seaman.Core
{
    /// <summary>
    /// Interface for hashing and checking password
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// produce hash string
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        String HashPassword(String password);

        /// <summary>
        /// verify hash string against plain text password
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Boolean VerifyHashedPassword(String hashedPassword, String password);
    }

    public delegate IPasswordHasher PasswordHasherFactory(String name);
}