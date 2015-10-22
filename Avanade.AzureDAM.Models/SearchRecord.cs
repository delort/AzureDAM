namespace Avanade.AzureDAM.Models
{
    public class SearchRecord
    {
        public string id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string ContentType { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string Keywords { get; set; }

        public string GetIndexUploadDocument()
        {
            return $@"{{
                ""value"" : [{{
                    ""@search.action"": ""upload"", 
                    ""id"" : ""{this.id}"", 
                    ""Name"" : ""{this.Name}"", 
                    ""Description"" : ""{this.Description}"", 
                    ""Author"" : ""{this.Author}"", 
                    ""Category"" : ""{this.Category}"", 
                    ""ContentType"" : ""{this.ContentType}"", 
                    ""Url"" : ""{this.Url}"", 
                    ""Keywords"" : ""{this.Keywords}""
                    }}]}}";
        }
    }
}