using System.Web;
using Avanade.AzureDAM.Integrations.Facades;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Commands
{
    public abstract class UploadCommand : Command<UploadCommandResult>
    {
        protected string Name;
        protected string Description;
        protected string Id;
        protected WebFile WebFile;

        private readonly UploadAssetNotifier _notifier;
        
        protected UploadCommand(UploadAssetNotifier notifier)
        {
            _notifier = notifier;
        }

        public abstract UploadCommandResult Do();

        public Command<UploadCommandResult> With(string name, string description, HttpPostedFileBase httpFile)
        {
            Name = name;
            Description = description;
            WebFile = new WebFile(httpFile);

            return this;
        }

        protected void SendNotifications(AssetMetadata metadata) => _notifier.SendNotifications(metadata);
    }
}