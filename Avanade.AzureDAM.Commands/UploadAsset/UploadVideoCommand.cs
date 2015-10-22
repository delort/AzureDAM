using System;
using System.Web;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Commands
{
    public class UploadVideoCommand : UploadCommand
    {
        private readonly MediaServicesRepository _mediaServicesRepository;
        private readonly AssetDocumentRepository _assetDocumentRepository;

        public UploadVideoCommand(MediaServicesRepository mediaServicesRepository, AssetDocumentRepository assetDocumentRepository, UploadAssetNotifier notifier) 
            : base(notifier)
        {
            _mediaServicesRepository = mediaServicesRepository;
            _assetDocumentRepository = assetDocumentRepository;
        }

        public override UploadCommandResult Do()
        {
            Id = Guid.NewGuid().ToString();

            var fileBytes = WebFile.ReadBytes();
            var fileName = $"original{WebFile.Extension}";

            var storageMetadata = _mediaServicesRepository.StoreAsset(Id, fileName, WebFile.ContentType, fileBytes);

            var metadata = StoreMetadata(storageMetadata);
            SendNotifications(metadata);

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
                Url = storageMetadata.StorageLocation,
                StorageMetadata = storageMetadata,
                Type = AssetTypeMapping.GetTypeFor(WebFile.ContentType)
            };

            _assetDocumentRepository.Create(assetMetadata);

            return assetMetadata;
        }
    }
}