using System.Configuration;
using Avanade.AzureDAM.Integrations.Facades;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Integrations.Repositories
{
    public class ImageFileRepository
    {
        private readonly BlobStorageFacade _blobStorage;
        private readonly string _containerName;

        public ImageFileRepository()
        {
             _containerName = ConfigurationManager.AppSettings["ImagesContainer"];
            var blobStorageConnectionString = ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
            _blobStorage = new BlobStorageFacade(blobStorageConnectionString);
          
        }

        public AssetStorageMetadata StoreAsset(string id, string fileName, string contentType, byte[] fileBytes)
        {
            if (fileBytes == null)
                return null;
            
            var storageFileName = _blobStorage.GetStorageFileName(id, fileName);

            var metadata = new AssetStorageMetadata
            {
                Id = id,
                StorageId = storageFileName               
            };
            
             _blobStorage.AddFileToBlobStorage(_containerName, metadata.StorageId, contentType, fileBytes);
            
            return metadata;           
        }

        public StoredAsset ReadImage(string storageName) => _blobStorage.GetFileFromBlobStorage(_containerName, storageName);
    }
}
