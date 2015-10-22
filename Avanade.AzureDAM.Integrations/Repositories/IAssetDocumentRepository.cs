using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Integrations.Repositories
{
    public interface IAssetDocumentRepository
    {
        void Create(AssetMetadata metadata);
        AssetMetadata Get(string id);
        void Update(AssetMetadata metadata);
    }
}