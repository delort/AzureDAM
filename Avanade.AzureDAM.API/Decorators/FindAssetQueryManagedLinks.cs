using System.Collections.Generic;
using System.Web;
using Avanade.AzureDAM.Models;
using Avanade.AzureDAM.Queries;
using Avanade.AzureDAM.Storage;

namespace Avanade.AzureDAM.API.App_Start
{
    public class FindAssetQueryManagedLinks : FindAssetQuery
    {
        public FindAssetQueryManagedLinks(AssetSearchRepository repository) : base(repository)
        {
        }

        public override IEnumerable<AssetSearchResult> Load()
        {
            var listOfResults = base.Load();
            var apiKey = HttpContext.Current.Request.Headers["api-key"];

            var overrideBaseUrl = $"http://localhost:20164/asset/{apiKey}/";
            var storageUrl = "https://azuredam.blob.core.windows.net/images/";

            foreach (var result in listOfResults)
            {
                var newUrl = result.Url.Replace(storageUrl, overrideBaseUrl);
                result.Url = newUrl;
            }

            return listOfResults;
        }
    }
}