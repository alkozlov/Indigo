using System;
using System.IO;
using System.Threading.Tasks;

namespace Indigo.BusinessLogicLayer.Storage
{
    public class StorageProvider
    {
        #region Defaults vaules

        private const String DefaultStorageFolder = "\\Storage\\";

        #endregion

        #region Singltone

        private static StorageProvider _current;

        public static StorageProvider Current
        {
            get { return _current ?? (_current = new StorageProvider()); }
        }

        #endregion

        #region Properties

        private String _storageLocation;

        #endregion

        #region Constructor

        private StorageProvider()
        {
            this._storageLocation = String.Concat(AppDomain.CurrentDomain.BaseDirectory, DefaultStorageFolder);
        }

        #endregion

        #region Methods

        public void SetStorageLocation(String storageLocation)
        {
            this._storageLocation = storageLocation;
        }

        /// <summary>
        /// Move file to storage.
        /// </summary>
        /// <param name="originalFilePath">Full original file name.</param>
        /// <param name="storageFileName">Only file name with extension.</param>
        public void AddFileToStorage(String originalFilePath, String storageFileName)
        {
            // Create storage directory if not exists
            DirectoryInfo storageDirectory = new DirectoryInfo(this._storageLocation);
            if (!storageDirectory.Exists)
            {
                storageDirectory.Create();
            }

            // Verify exists tagret file
            FileInfo targetFileInfo = new FileInfo(originalFilePath);
            if (!targetFileInfo.Exists)
            {
                throw new FileNotFoundException("Can't find file to move in storage.", originalFilePath);
            }

            // Move file to storage
            String storageFilePath = String.Concat(this._storageLocation, "\\", storageFileName);
            File.Copy(originalFilePath, storageFilePath);
        }

        #endregion
    }
}