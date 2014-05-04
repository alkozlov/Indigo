namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;


    public class CheckSumCollection : ReadOnlyCollection<KeyValuePair<UInt32, Shingle>>
    {
        public Int32? DocumentId { get; private set; }

        private CheckSumCollection(IList<KeyValuePair<UInt32, Shingle>> list, Int32? documentId)
            : base(list)
        {
            this.DocumentId = documentId;
        }

        public static async Task<CheckSumCollection> CreateAsync(ShingleList shingles, HashAlgorithmType hashAlgorithmType)
        {
            CheckSumCollection checkSumCollection = await Task<CheckSumCollection>.Run(() =>
            {
                List<KeyValuePair<UInt32, Shingle>> checkSumList = new List<KeyValuePair<UInt32, Shingle>>();
                foreach (var shingle in shingles)
                {
                    Shingle businesShingle = new Shingle(shingle.Words)
                    {
                        ShingleId = shingle.ShingleId,
                        CheckSum = shingle.CheckSum
                    };

                    UInt32 checkSum = ComputeCheckSum(businesShingle, hashAlgorithmType);
                    checkSumList.Add(new KeyValuePair<UInt32, Shingle>(checkSum, businesShingle));
                }

                return new CheckSumCollection(checkSumList, shingles.DocumentId);
            });

            return checkSumCollection;
        }

        public async Task SaveAsync()
        {
            List<DataModels.Shingle> dataShingles = this.Items.Where(item => this.DocumentId.HasValue)
                .Select(item => new DataModels.Shingle
                {
                    DocumentId = this.DocumentId.Value,
                    ShingleSize = item.Value.Size,
                    CheckSum = item.Key
                }).ToList();

            using (IShinglesRepository shinglesRepository = new ShinglesRepository())
            {
                await shinglesRepository.CreateShinglesAsync(dataShingles);
            }
        }

        #region Helpers

        private static UInt32 ComputeCheckSum(Shingle shingle, HashAlgorithmType hashAlgorithmType)
        {
            IHashAlgorithm hasher = HasherFactory.GetHasherInstance(hashAlgorithmType);
            UInt32 checkSum = hasher.ComputeCheckSum(shingle.AsString);

            return checkSum;
        }

        #endregion
    }
}