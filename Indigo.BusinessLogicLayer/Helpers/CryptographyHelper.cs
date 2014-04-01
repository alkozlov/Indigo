using System;
using System.Security.Cryptography;

namespace Indigo.BusinessLogicLayer.Helpers
{
    public class CryptographyHelper
    {
        /// <summary>
        /// Generate salt 128 symbols by Default.
        /// </summary>
        /// <param name="expectedStringLength"></param>
        /// <returns></returns>
        public static String GenerateRandomBase64String(Int32 expectedStringLength)
        {
            Byte[] data = new Byte[expectedStringLength];
            RNGCryptoServiceProvider rngCryptoService = new RNGCryptoServiceProvider();
            rngCryptoService.GetNonZeroBytes(data);
            String passwordSalt = Convert.ToBase64String(data).Substring(0, expectedStringLength);
            return passwordSalt;
        }
    }

}