namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class RIPEMD160Hasher : IHashAlgorithm
    {
        public uint ComputeCheckSum(string text)
        {
            RIPEMD160Managed ripemd160 = new RIPEMD160Managed();
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            Byte[] hashInBytesFormat = ripemd160.ComputeHash(textInBytesFormat);

            UInt32 result = BitConverter.ToUInt32(hashInBytesFormat, 0);
            return result;
        }
    }
}