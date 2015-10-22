using System.Collections;
using System.Collections.Generic;
using Avanade.AzureDAM.Models;
using Avanade.AzureDAM.Storage;

namespace Avanade.AzureDAM.Queries
{
    public class FindAssetQuery: Query<IEnumerable<AssetSearchResult>>
    {
        private readonly AssetSearchRepository _repository;
        private string _query;

        public FindAssetQuery(AssetSearchRepository repository)
        {
            _repository = repository;
        }

        public override IEnumerable<AssetSearchResult> Load()
        {
            return _repository.Find(_query);
        }

        public Query<IEnumerable<AssetSearchResult>> For(string query)
        {
            _query = query;
            return this;
        }
    }
}