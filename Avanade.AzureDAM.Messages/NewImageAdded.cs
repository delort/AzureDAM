namespace Avanade.AzureDAM.Messages
{
    public class NewImageAdded : Message
    {
        public string ImageName { get; set; }
        public string Id { get; set; }
    }
}