namespace Avanade.AzureDAM.Messages
{
    public class ChannelAssetError : Message
    {
        public string AssetName { get; set; }

        public string ChannelName { get; set; }

        public string Reason { get; set; }
    }
}