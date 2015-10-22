using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanade.AzureDAM.DemoSetup
{
    public static class ImageMetaDataExtensions
    {
        public static string GetValue(this IReadOnlyList<MetadataExtractor.Directory> imageMetadata, string directoryName, string tagName)
        {
            return imageMetadata.SingleOrDefault(dir => dir.Name == directoryName)
                                               ?.Tags.SingleOrDefault(tag => tag.TagName == tagName)
                                               ?.Description;
        }
    }
}
