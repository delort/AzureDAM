using System.Configuration;
using System.Data.Common;
using System.Linq;
using Avanade.AzureDAM.Integrations.Facades;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Integrations.Repositories
{
    public class AssetDocumentRepository : IAssetDocumentRepository
    {
        private readonly string _databaseName;
        private readonly string _collectioName = "Assets";
        

        public AssetDocumentRepository()
        {           
            _databaseName = ConfigurationManager.AppSettings["DocumentDBDatabaseName"];
            _collectioName = ConfigurationManager.AppSettings["AssetCollectionName"];
        }

        public AssetMetadata Get(string id)
        {
            using (var context = new DocumentDbContext())
            {
                context.OpenDatabase(_databaseName);
                var query = context.CreateDocumentQuery<AssetMetadata>(_collectioName)
                                   .Where(asset => asset.Id == id);

                return query.AsEnumerable().FirstOrDefault();
            }
        }

        public void Create(AssetMetadata metadata)
        {
            using (var context = new DocumentDbContext())
            {
                context.OpenDatabase(_databaseName);
                context.CreateDocument(metadata, _collectioName);
            }
        }

        public void Update(AssetMetadata metadata)
        {
            using (var context = new DocumentDbContext())
            {
                context.OpenDatabase(_databaseName);
                context.UpdateDocument(metadata.Id, metadata, _collectioName);
            }
        }
    }
}
