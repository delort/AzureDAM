using Avanade.AzureDAM.Integrations.Clients;

namespace Avanade.AzureDAM.DemoSetup.Search
{
    public static class SearchCleanUp
    {
        private static SearchManagementClient _client;

        public static void Run()
        {
            _client = new SearchManagementClient();
            _client.DeleteIndex(SearchConfiguration.IndexName);
            _client.DeleteDataSource(SearchConfiguration.DatasourceName);
            _client.DeleteIndexer(SearchConfiguration.IndexerName);
        }

        
    }
}