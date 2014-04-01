using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Indigo.BusinessLogicLayer.Helpers
{
    public class PasswordHelper
    {
        public const Int32 DefaultPasswordSaltLength = 128;

        /// <summary>
        /// Generate password salt 128 characters length by default
        /// </summary>
        /// <param name="saltLength"></param>
        /// <returns></returns>
        public static String GeneratePasswordSalt(Int32? saltLength = null)
        {
            String salt = CryptographyHelper.GenerateRandomBase64String(saltLength ?? DefaultPasswordSaltLength);

            return salt;
        }

        /// <summary>
        /// Compute password hash
        /// </summary>
        /// <param name="originalPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static String ComputePasswordHash(String originalPassword, String salt)
        {
            if (String.IsNullOrEmpty(originalPassword))
            {
                throw new ArgumentNullException("originalPassword", "Original password can't be null or empty.");
            }

            if (String.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException("salt", "Salt can't be null or empty.");
            }

            String passwordHash = HashPassword(originalPassword, salt);

            return passwordHash;
        }

        private static String HashPassword(String originalPassword, String salt)
        {
            MD5CryptoServiceProvider md5CryptoService = new MD5CryptoServiceProvider();
            Byte[] originaPasswordBytes = Encoding.UTF8.GetBytes(originalPassword);
            Byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            Byte[] commonBytes = saltBytes.Concat(originaPasswordBytes).ToArray();
            Byte[] hashBytes = md5CryptoService.ComputeHash(commonBytes);
            String hashPassword = Convert.ToBase64String(hashBytes);

            return hashPassword;
        }
    }

}