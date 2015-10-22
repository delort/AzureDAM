using Avanade.AzureDAM.Integrations.Facades;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Integrations.Clients
{
    public class SearchManagementClient
    {
        private readonly SearchManagementEndpoints _endpoints;
        
        public SearchManagementClient()
        {
            _endpoints = new SearchManagementEndpoints();
        }

        public void CreateDataSource(string datasourceDefinition) => new SearchWebRequestFacade(_endpoints.SearchDataSources(), "POST", datasourceDefinition)
            .Invoke();

        public void DeleteDataSource(string dataSourceName) => new SearchWebRequestFacade(_endpoints.SearchDataSource(dataSourceName), "DELETE")
            .Invoke();

        public void DeleteIndexer(string indexerName) => new SearchWebRequestFacade(_endpoints.SearchIndexer(indexerName), "DELETE")
            .Invoke();

        public void CreateIndexer(string indexerDefinition) => new SearchWebRequestFacade(_endpoints.SearchIndexers(), "POST", indexerDefinition)
            .Invoke();

        public void DeleteIndex(string indexName) => new SearchWebRequestFacade(_endpoints.SearchIndex(indexName), "DELETE")
            .Invoke();

        public void CreateIndex(string indexDefinition) => new SearchWebRequestFacade(_endpoints.SearchIndexes(), "POST", indexDefinition)
            .Invoke();

        public void RunIndexer(string indexerName) => new SearchWebRequestFacade(_endpoints.SearchIndexer(indexerName + "/run"), "POST")
            .Invoke();

        public void AddSearchRecord(string indexName, SearchRecord searchRecord) => new SearchWebRequestFacade(_endpoints.SearchDocument(indexName), "POST", searchRecord.GetIndexUploadDocument())
            .Invoke();
    }
}