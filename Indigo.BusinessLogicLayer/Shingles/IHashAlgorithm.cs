namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;

    public interface IHashAlgorithm
    {
        UInt32 ComputeCheckSum(String text);
    }
}