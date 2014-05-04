namespace Indigo.BusinessLogicLayer.Storage
{
    using System;

    public class StorageConnector
    {
        private const String LocalStorageDirectoryAbsolutePath = "\\Indigo\\LocalStorage\\";

        private StorageConnector()
        {
            
        }

        public static StorageConnection GetStorageConnection(StorageType storageType)
        {
            StorageConnection storageConnection = null;

            switch (storageType)
            {
                case StorageType.Local:
                {
                    String localStorageDirectory = GetLocalStoragePath();
                    storageConnection = new LocalStorageConnection(localStorageDirectory);
                } break;

                case StorageType.Server:
                {
                    String serverStorageDirectory = GetServerStoragePath();
                    storageConnection = new ServerStorageConnection(serverStorageDirectory);
                } break;
            }

            return storageConnection;
        }

        private static String GetLocalStoragePath()
        {
            String commonApplicationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            String localStorageDirectory = String.Concat(commonApplicationDirectory, "\\", LocalStorageDirectoryAbsolutePath);

            return localStorageDirectory;
        }

        private static String GetServerStoragePath()
        {
            return "127.0.0.1:80\\indigostorage\\";
        }
    }
}