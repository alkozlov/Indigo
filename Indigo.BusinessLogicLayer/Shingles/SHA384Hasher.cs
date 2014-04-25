namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class SHA384Hasher : IHashAlgorithm
    {
        public uint ComputeCheckSum(string text)
        {
            SHA384CryptoServiceProvider sha384 = new SHA384CryptoServiceProvider();
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            Byte[] hashInBytesFormat = sha384.ComputeHash(textInBytesFormat);

            UInt32 result = BitConverter.ToUInt32(hashInBytesFormat, 0);
            return result;
        }
    }
}