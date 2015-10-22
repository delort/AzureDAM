using Avanade.AzureDAM.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Avanade.AzureDAM.Integrations.Facades
{
    public class BlobStorageFacade
    {

        private readonly string _blobStorageConnectionString;
    

        public BlobStorageFacade(string blobStorageConnectionString)
        {
            this._blobStorageConnectionString = blobStorageConnectionString;
            
        }

        public CloudBlobContainer GetBlobContainer(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_blobStorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            return container;
        }

        public void DeleteContainer(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_blobStorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);

            container.DeleteIfExists();
        }

        public void AddFileToBlobStorage(string containerName, string storageName, string contentType, byte[] data)
        {
            var container = GetBlobContainer(containerName);
            var blockBlob = container.GetBlockBlobReference(storageName);
            blockBlob.Properties.ContentType = contentType;            
            blockBlob.UploadFromByteArray(data, 0, data.Length);
        }

        public StoredAsset GetFileFromBlobStorage(string containerName, string storageName)
        {
            var container = GetBlobContainer(containerName);
            var blockBlob = container.GetBlobReference(storageName);
            var blobContents = blockBlob.OpenRead();

            return new StoredAsset()
            {
                Id = GetStorageIdSegment(blockBlob.Name),
                Name = GetStorageNameSegment(blockBlob.Name),
                Extension = GetStorageExtensionSegment(blockBlob.Name),
                //TODO: Contenttype is empty for some reason.
                ContentType = blockBlob.Properties.ContentType,
                FileStream = blobContents
            };
        }


        public string GetStorageNameSegment(string name) => name.Substring(name.IndexOf("/") + 1);
        public string GetStorageIdSegment(string name) => name.Substring(0, name.IndexOf("/"));
        public string GetStorageExtensionSegment(string name) => name.Substring(name.LastIndexOf("."));

        public string GetStorageFileName(string storageId, string fileName) => $"{storageId}/{fileName}";
    }
}
