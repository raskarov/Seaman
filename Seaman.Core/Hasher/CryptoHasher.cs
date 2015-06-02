using System;
using System.Linq;
using System.Security.Cryptography;

namespace Seaman.Core
{
    /// <summary>
    /// Asp.Net Identity framework compatible hasher
    /// </summary>
    public class CryptoHasher : IPasswordHasher
    {

        private const int PBKDF2IterCount = 1000;
        private const int PBKDF2SubkeyLength = 32;
        private const int SaltSize = 16;


        public String HashPassword(String password)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] salt;
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, PBKDF2IterCount))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            byte[] inArray = new byte[SaltSize + PBKDF2SubkeyLength + 1];
            Buffer.BlockCopy(salt, 0, inArray, 1, SaltSize);
            Buffer.BlockCopy(bytes, 0, inArray, SaltSize + 1, PBKDF2SubkeyLength);
            return Convert.ToBase64String(inArray);
        }

        public Boolean VerifyHashedPassword(String hashedPassword, String password)
        {
            if (hashedPassword == null)
                return false;
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] numArray = Convert.FromBase64String(hashedPassword);
            if (numArray.Length != SaltSize + PBKDF2SubkeyLength + 1 || numArray[0] != 0)
                return false;
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(numArray, 1, salt, 0, SaltSize);
            byte[] a = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(numArray, SaltSize + 1, a, 0, PBKDF2SubkeyLength);
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
                bytes = rfc2898DeriveBytes.GetBytes(PBKDF2SubkeyLength);
            return a.SequenceEqual(bytes);
        }
    }
}