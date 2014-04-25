namespace Indigo.BusinessLogicLayer.Shingles
{
    public class HasherFactory
    {
        public static IHashAlgorithm GetHasherInstance(HashAlgorithmType hashAlgorithmType)
        {
            IHashAlgorithm hasher;
            switch (hashAlgorithmType)
            {
                case HashAlgorithmType.Crc32:
                    {
                        hasher = new Crc32Hasher();
                    } break;

                case HashAlgorithmType.MD5:
                    {
                        hasher = new MD5Hasher();
                    } break;

                case HashAlgorithmType.SHA1:
                    {
                        hasher = new SHA1Hasher();
                    } break;

                case HashAlgorithmType.SHA256:
                    {
                        hasher = new SHA256Hasher();
                    } break;

                case HashAlgorithmType.SHA384:
                    {
                        hasher = new SHA384Hasher();
                    } break;

                case HashAlgorithmType.SHA512:
                    {
                        hasher = new SHA512Hasher();
                    } break;

                case HashAlgorithmType.RIPEMD160:
                    {
                        hasher = new RIPEMD160Hasher();
                    } break;

                default:
                    {
                        hasher = new Crc32Hasher();
                    } break;
            }

            return hasher;
        }
    }
}