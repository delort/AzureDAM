using System.Runtime.Remoting.Messaging;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Queries
{
    public class GetAssetFileQuery : Query<StoredAsset>
    {
        private readonly ImageFileRepository _repository;
        private string _storageName;
        private string _channel;

        public GetAssetFileQuery(ImageFileRepository repository)
        {
            _repository = repository;
        }


        public GetAssetFileQuery For(string channel, string storageName)
        {
            _storageName = storageName;
            _channel = channel;

            return this;
        }

        public override StoredAsset Load()
        {
            try
            {
                return _repository.ReadImage(_storageName);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}