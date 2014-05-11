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


    public class CheckSumCollection : ReadOnlyCollection<KeyValuePair<long, Shingle>>
    {
        public Int32? DocumentId { get; private set; }

        private CheckSumCollection(IList<KeyValuePair<long, Shingle>> list, Int32? documentId)
            : base(list)
        {
            this.DocumentId = documentId;
        }

        public static async Task<CheckSumCollection> CreateAsync(ShingleList shingles, HashAlgorithmType hashAlgorithmType)
        {
            CheckSumCollection checkSumCollection = await Task.Run(() =>
            {
                List<KeyValuePair<long, Shingle>> checkSumList = new List<KeyValuePair<long, Shingle>>();
                foreach (var shingle in shingles)
                {
                    Shingle businessShingle = new Shingle(shingle.Words)
                    {
                        ShingleId = shingle.ShingleId,
                        CheckSum = shingle.CheckSum
                    };

                    long checkSum = ComputeCheckSum(businessShingle, hashAlgorithmType);
                    checkSumList.Add(new KeyValuePair<long, Shingle>(checkSum, businessShingle));
                }

                return new CheckSumCollection(checkSumList, shingles.DocumentId);
            });

            return checkSumCollection;
        }

        public static CheckSumCollection Create(ShingleList shingles)
        {
            List<KeyValuePair<long, Shingle>> checkSumList = new List<KeyValuePair<long, Shingle>>();
            foreach (var shingle in shingles)
            {
                Shingle businessShingle = new Shingle(shingle.Words)
                {
                    ShingleId = shingle.ShingleId,
                    CheckSum = shingle.CheckSum
                };

                checkSumList.Add(new KeyValuePair<long, Shingle>(shingle.CheckSum ?? 0, businessShingle));
            }

            CheckSumCollection checkSumCollection = new CheckSumCollection(checkSumList, shingles.DocumentId);
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

        private static long ComputeCheckSum(Shingle shingle, HashAlgorithmType hashAlgorithmType)
        {
            IHashAlgorithm hasher = HasherFactory.GetHasherInstance(hashAlgorithmType);
            UInt32 checkSum = hasher.ComputeCheckSum(shingle.AsString);

            return checkSum;
        }

        #endregion
    }
}