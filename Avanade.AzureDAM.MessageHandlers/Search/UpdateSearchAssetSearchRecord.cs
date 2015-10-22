using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.MessageHandlers
{
    public class UpdateSearchAssetSearchRecord : IHandle<NewImageAdded>, IHandle<NewVideoAdded>
    {
        private readonly AssetDocumentRepository _docuementRepository;
        private readonly SearchManagementClient _searchClient;

        public UpdateSearchAssetSearchRecord(AssetDocumentRepository docuementRepository, SearchManagementClient _searchClient)
        {
            _docuementRepository = docuementRepository;
            this._searchClient = _searchClient;
        }

        public void Handle(NewImageAdded message) => AddAssetIndex(message.Id);

        public void Handle(NewVideoAdded message) => AddAssetIndex(message.Id);

        private void AddAssetIndex(string assetId)
        {
            var asset = _docuementRepository.Get(assetId);
            var searchRecord = new SearchRecord
            {
                id = asset.Id,
                Author = asset.Author,
                Category = asset.Category,
                ContentType = asset.ContentType,
                Description = asset.Description,
                Keywords = asset.Keywords,
                Name = asset.Name,
                Type = asset.Type,
                Url = asset.Url
            };

            _searchClient.AddSearchRecord("defaultassetindex", searchRecord);
        }
    }
}