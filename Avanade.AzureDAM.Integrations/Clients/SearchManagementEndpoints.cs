using System.Configuration;
using System.Web.UI.WebControls.WebParts;

namespace Avanade.AzureDAM.Integrations.Clients
{
    public class SearchManagementEndpoints
    {

        private static string _searchUrl;
        private const string ApiVersion = "?api-version=2015-02-28-Preview";
        private const string SearchBaseUri = "https://{0}.search.windows.net";

        public SearchManagementEndpoints()
        {
            _searchUrl = string.Format(SearchManagementEndpoints.SearchBaseUri, ConfigurationManager.AppSettings["SearchServiceName"]);
        }

        public string BuildManagementEndpoint(string template, params object[] prms)
        {
            return string.Format(template.Replace("$url$", _searchUrl), prms) + ApiVersion;
        }

        public string SearchDataSources() => BuildManagementEndpoint(Templates.SearchDataSourcesUri);
        public string SearchDataSource(string dataSourceName) => BuildManagementEndpoint(Templates.SearchDataSourceUri, dataSourceName);
        public string SearchIndexers() => BuildManagementEndpoint(Templates.SearchIndexersUri);
        public string SearchIndexer(string indexerName) => BuildManagementEndpoint(Templates.SearchIndexerUri, indexerName);
        public string SearchIndexes() => BuildManagementEndpoint(Templates.SearchIndexesUri);
        public string SearchIndex(string indexName) => BuildManagementEndpoint(Templates.SearchIndexUri);
        public string SearchDocument(string indexName) => BuildManagementEndpoint(Templates.SearchDocumentUri, indexName);


        private static class Templates
        {
            public const string SearchDataSourcesUri = "$url$/datasources";
            public const string SearchDataSourceUri = "$url$/datasources/{0}";
            public const string SearchIndexersUri = "$url$/indexers";
            public const string SearchIndexerUri = "$url$/indexers/{0}";
            public const string SearchIndexesUri = "$url$/indexes";
            public const string SearchIndexUri = "$url$/indexes/{0}";
            public const string SearchDocumentUri = "$url$/indexes/{0}/docs/index";
        }
    }
}
