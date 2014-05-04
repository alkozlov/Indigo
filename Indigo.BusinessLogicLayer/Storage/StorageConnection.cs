namespace Indigo.BusinessLogicLayer.Storage
{
    using System;
    using System.IO;

    public abstract class StorageConnection : IDisposable
    {
        #region Properties

        public abstract StorageType StorageLocation { get; }

        public abstract Boolean IsAvailable { get; }

        public String StorageDirectory { get; private set; }

        #endregion

        #region Methods

        public abstract FileInfo GetFileInfo(String storageFileName);

        public abstract void UploadFile(String localFileFullName, String storageFileName);

        #endregion

        #region Constructors

        protected internal StorageConnection(String storageDirectory)
        {
            this.StorageDirectory = storageDirectory;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion
    }
}