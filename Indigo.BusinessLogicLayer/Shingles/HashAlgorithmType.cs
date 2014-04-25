namespace Indigo.BusinessLogicLayer.Shingles
{
    public enum HashAlgorithmType : byte
    {
        Crc32 = 0,
        MD5 = 1,
        SHA1 = 2,
        SHA256 = 3,
        SHA384 = 4,
        SHA512 = 5,
        RIPEMD160 = 6
    }
}