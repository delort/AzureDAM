using System.Runtime;

namespace Avanade.AzureDAM.Messages
{
    public class NewVideoAdded : Message
    {
        public string Id { get; set; }

        public string AssetId { get; set; }
    }
}