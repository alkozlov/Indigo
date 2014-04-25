namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class SHA256Hasher : IHashAlgorithm
    {
        public uint ComputeCheckSum(string text)
        {
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            Byte[] hashInBytesFormat = sha256.ComputeHash(textInBytesFormat);

            UInt32 result = BitConverter.ToUInt32(hashInBytesFormat, 0);
            return result;
        }
    }
}