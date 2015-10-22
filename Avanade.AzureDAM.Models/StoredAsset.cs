using System.IO;
using System.Runtime;

namespace Avanade.AzureDAM.Models
{
    public class StoredAsset
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public Stream FileStream { get; set; }
    }
}