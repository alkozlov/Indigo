namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class SHA1Hasher : IHashAlgorithm
    {
        public uint ComputeCheckSum(string text)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            Byte[] hashInBytesFormat = sha1.ComputeHash(textInBytesFormat);

            UInt32 result = BitConverter.ToUInt32(hashInBytesFormat, 0);
            return result;
        }
    }
}