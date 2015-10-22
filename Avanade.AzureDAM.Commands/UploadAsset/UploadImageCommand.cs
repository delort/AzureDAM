using Avanade.AzureDAM.Models;
using System;
using System.Configuration;
using System.Web;
using Avanade.AzureDAM.Integrations.Facades;
using Avanade.AzureDAM.Integrations.Repositories;

namespace Avanade.AzureDAM.Commands
{
    public class UploadImageCommand : UploadCommand
    {
        private readonly ImageFileRepository _imageFileRepository;
        private readonly AssetDocumentRepository _assetDocumentRepository;

        private readonly string _assetRootPath;

        public UploadImageCommand(ImageFileRepository imageFileRepository, AssetDocumentRepository assetDocumentRepository, UploadAssetNotifier notifier)
            :base(notifier)
        {
            _assetRootPath = ConfigurationManager.AppSettings["ImageRootPath"];
            _imageFileRepository = imageFileRepository;
            _assetDocumentRepository = assetDocumentRepository;
        }

        public override UploadCommandResult Do()
        {
            Id = Guid.NewGuid().ToString();
            var storageMetadata = StoreAsset();

            if (storageMetadata == null)
                return null;

            var assetMetadata = StoreMetadata(storageMetadata);

            SendNotifications(assetMetadata);

            return new UploadCommandResult(Id);
        }

        private AssetMetadata StoreMetadata(AssetStorageMetadata storageMetadata)
        {
            var assetMetadata = new AssetMetadata
            {
                Id = Id,
                Name = Name,
                Description = Description,
                ContentType = WebFile.ContentType,
                Url = $"{_assetRootPath}/{storageMetadata.StorageId}",
                StorageMetadata = storageMetadata,
                Type = AssetTypeMapping.GetTypeFor(WebFile.ContentType)
            };

            _assetDocumentRepository.Create(assetMetadata);

            return assetMetadata;
        }

        private AssetStorageMetadata StoreAsset()
        {
            var fileBytes = WebFile.ReadBytes();
            var fileName = $"original{WebFile.Extension}";

            var storageMetadata = _imageFileRepository.StoreAsset(Id, fileName, WebFile.ContentType, fileBytes);
            
            return storageMetadata;
        }
    }
}
