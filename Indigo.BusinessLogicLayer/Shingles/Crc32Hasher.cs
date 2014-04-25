namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Text;

    public class Crc32Hasher : IHashAlgorithm
    {
        public UInt32 ComputeCheckSum(String text)
        {
            Byte[] textInBytesFormat = Encoding.UTF8.GetBytes(text);
            UInt32 result = Crc32.Compute(textInBytesFormat);
            return result;
        }
    }
}