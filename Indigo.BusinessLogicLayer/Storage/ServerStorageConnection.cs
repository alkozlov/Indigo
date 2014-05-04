namespace Indigo.BusinessLogicLayer.Storage
{
    using System;
    using System.IO;
    using System.Security.AccessControl;

    public class ServerStorageConnection : StorageConnection
    {
        private const String DefaultServerDirectory = @"127.0.0.1:80\indigostorage\";

        protected internal ServerStorageConnection(String storageDirectory) : base(storageDirectory)
        {
        }

        public override StorageType StorageLocation
        {
            get { return StorageType.Server; }
        }

        public override Boolean IsAvailable
        {
            get
            {
                Boolean allow = false;
                Boolean deny = false;
                DirectorySecurity directorySecurity;
                try
                {
                    directorySecurity = Directory.GetAccessControl(base.StorageDirectory);
                }
                catch (DirectoryNotFoundException)
                {
                    return false;
                }
                if (directorySecurity == null)
                {
                    return false;
                }
                AuthorizationRuleCollection authorizationRuleCollection =
                    directorySecurity.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
                if (authorizationRuleCollection.Count == 0)
                {
                    return false;
                }
                foreach (FileSystemAccessRule rule in authorizationRuleCollection)
                {
                    if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    {
                        continue;
                    }
                    if (rule.AccessControlType == AccessControlType.Allow)
                    {
                        allow = true;
                    }
                    else
                    {
                        if (rule.AccessControlType == AccessControlType.Deny)
                        {
                            deny = true;
                        }
                    }
                }
                return allow && !deny;
            }
        }

        public override FileInfo GetFileInfo(String storageFileName)
        {
            FileInfo fileInfo = new FileInfo(storageFileName);
            return fileInfo.Exists ? fileInfo : null;
        }

        public override void UploadFile(String localFileFullName, String storageFileName)
        {
            // Verify exists tagret file
            FileInfo targetFileInfo = new FileInfo(localFileFullName);
            if (!targetFileInfo.Exists)
            {
                throw new FileNotFoundException("Can't find file on local disk.", localFileFullName);
            }

            // Move file to storage
            String storageFileFullName = String.Concat(base.StorageDirectory, "\\", storageFileName);
            File.Copy(localFileFullName, storageFileFullName);
        }
    }
}