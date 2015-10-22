using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanade.AzureDAM.DemoSetup
{
    public class FileMetadata
    {
        public int Size { get; set; }
        public string FileName { get; set; }

        public string FileLocation { get; set; }

        public string Photographer { get; set; }
        public string Area { get; set; }
        public string Category { get; set; }
    }
}
