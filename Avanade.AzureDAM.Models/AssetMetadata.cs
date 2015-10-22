using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanade.AzureDAM.Models
{
    public class AssetMetadata
    {
        public AssetMetadata()
        {
            Derivates = new List<Derivate>();
            Attributes = new Dictionary<string, string>();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string ContentType { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }
        
        //TODO: Make published matter (and add a post step to step published to true when all messages have been handled in the bus)
        public bool Published { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public AssetStorageMetadata StorageMetadata { get; set;  }
        
        //TODO: Add derivates as they get created
        public List<Derivate> Derivates { get; set; }
        public string Keywords { get; set; }
    }

    public class Derivate
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
