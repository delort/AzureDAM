namespace Avanade.AzureDAM.Commands
{
    public class UploadCommandResult : CommandResult
    {
        public UploadCommandResult(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}