 // ReSharper disable once CheckNamespace

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Avanade.AzureDAM.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

// ReSharper disable once CheckNamespace
namespace Avanade.AzureDAM.Storage
{
    public class AssetSearchRepository
    {
        private readonly SearchIndexClient _index;

        public AssetSearchRepository()
        {
            var searchService = ConfigurationManager.AppSettings["SearchServiceName"];
            var searchKey = ConfigurationManager.AppSettings["SearchServiceKey"];

            var defaultIndex = ConfigurationManager.AppSettings["DefaultSearchIndex"];
            var client = new SearchServiceClient(searchService, new SearchCredentials(searchKey));
            _index = client.Indexes.GetClient(defaultIndex);
        }


        public IEnumerable<AssetSearchResult> Find(string query)
        {
            var parameters = new SearchParameters();
            var response = _index.Documents.Search<AssetSearchResult>(query, parameters);

            return response.OrderByDescending(result => result.Score)
                           .Select(result => result.Document)
                           .ToList();
        }
    }
}