using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Integrations.Decorators
{
    public class AssetDocumentCDNDecorator : IAssetDocumentRepository
    {
        private readonly AssetDocumentRepository _repository;
        private const string CDNEndpoint = "https://az820680.vo.msecnd.net/";
        private const string StorageEndpoint = "https://azuredam.blob.core.windows.net/";

        public AssetDocumentCDNDecorator(AssetDocumentRepository repository)
        {
            _repository = repository;
        }


        public void Create(AssetMetadata metadata)
        {
            _repository.Create(metadata);   
        }

        public AssetMetadata Get(string id)
        {
            var asset = _repository.Get(id);
            asset.Url = asset.Url.Replace(StorageEndpoint, CDNEndpoint);

            return asset;
        }

        public void Update(AssetMetadata metadata)
        {
            _repository.Update(metadata);   
        }
    }
}