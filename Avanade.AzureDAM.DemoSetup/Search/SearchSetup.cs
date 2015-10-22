using System.Configuration;
using Avanade.AzureDAM.Integrations.Clients;

namespace Avanade.AzureDAM.DemoSetup.Search
{
    public class SearchSetup
    {
        private static SearchManagementClient _client;
        
        public static void Run()
        {
            _client = new SearchManagementClient();
            //CreateIndex();
            //CreateDataSource();
            CreateIndexer();
        }

        private static void CreateIndex()
        {
            var indexDefinition = SearchConfiguration.BuildIndexDefinition();
            _client.CreateIndex(indexDefinition);
        }

        private static void CreateIndexer()
        {
            var indexerDefinition = SearchConfiguration.BuildIndexerDefinition();
            _client.CreateIndexer(indexerDefinition);
        }

        private static void CreateDataSource()
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["DocumentDBConnectionString"] + ";Database=AssetMetadata";
            var datasourceDefinition = SearchConfiguration.BuildDataSourceDefinition(dbConnectionString);
            _client.CreateDataSource(datasourceDefinition);
        }
    }
}