using Avanade.AzureDAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avanade.AzureDAM.Integrations.Repositories;

namespace Avanade.AzureDAM.Queries
{
    public class GetAssetDetailsQuery : Query<AssetMetadata>
    {
        private AssetDocumentRepository _assetDocumentRepository;
        private string _idForAssetToLoad;

        public GetAssetDetailsQuery(AssetDocumentRepository assetdocumentRepository)
        {
            _assetDocumentRepository = assetdocumentRepository;
        }

        public GetAssetDetailsQuery For(string id)
        {
            _idForAssetToLoad = id;

            return this;
        }

        public override AssetMetadata Load()
        {
            return  _assetDocumentRepository.Get(_idForAssetToLoad);
        }
    }
}
