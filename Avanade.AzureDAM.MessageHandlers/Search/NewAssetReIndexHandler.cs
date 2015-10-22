using System.Diagnostics;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Messages;

namespace Avanade.AzureDAM.MessageHandlers
{
    public class NewAssetReIndexHandler // : IHandle<NewVideoAdded>, IHandle<NewImageAdded>
    {
        private readonly SearchManagementClient _searchClient;

        public NewAssetReIndexHandler(SearchManagementClient searchClient)
        {
            _searchClient = searchClient;
        }

        public void Handle(NewVideoAdded message)
        {
            RunMediaAssetIndexer();
        }

        public void Handle(NewImageAdded message)
        {
            RunMediaAssetIndexer();
        }

        private void RunMediaAssetIndexer()
        {
            _searchClient.RunIndexer("assetindexer");
        }
    }
}