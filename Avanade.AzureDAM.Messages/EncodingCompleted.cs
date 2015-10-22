namespace Avanade.AzureDAM.Messages
{
    public class EncodingCompleted : Message
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public string JobId { get; set; }
    }
}