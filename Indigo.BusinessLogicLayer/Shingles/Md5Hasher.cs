namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class MD5Hasher : IHashAlgorithm
    {
        public UInt32 ComputeCheckSum(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            Byte[] hashInBytesFormat = md5.ComputeHash(textInBytesFormat);

            UInt32 result = BitConverter.ToUInt32(hashInBytesFormat, 0);
            return result;
        }
    }
}