namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class SHA512Hasher : IHashAlgorithm
    {
        public uint ComputeCheckSum(string text)
        {
            SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider();
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            Byte[] hashInBytesFormat = sha512.ComputeHash(textInBytesFormat);

            UInt32 result = BitConverter.ToUInt32(hashInBytesFormat, 0);
            return result;
        }
    }
}