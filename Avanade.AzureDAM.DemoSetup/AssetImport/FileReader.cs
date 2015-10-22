using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanade.AzureDAM.DemoSetup
{
    public class FileReader
    {
        private const string FileLocationPath = @"Z:\Photos\PhotoCompetition\";
        private const string MetadataFileLocation = FileLocationPath + @"metadata.csv";

        public static IEnumerable<FileMetadata> ReadMetadataFile()
        {
            var metadataList = from line in File.ReadAllLines(MetadataFileLocation).Skip(1)
                               let columns = line.Split(';')
                               select new FileMetadata
                               {
                                   Size = int.Parse(columns[0]),
                                   FileName = GetFileName(columns[1]),
                                   FileLocation = string.Format("{0}\\{1}", FileLocationPath, GetFileName(columns[1])),
                                   Photographer = columns[2],
                                   Area = columns[4],
                                   Category = columns[7]
                               };

            return metadataList.ToList();
        }

        public static byte[] ReadFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        public static FileInfo GetFileInfo(string filePath)
        {
            return new FileInfo(filePath);
        }

        private static string GetFileName(string sharePointPath)
        {
            return sharePointPath.Replace("sites/GoOrangePhotoCompetition/MTJAC_15/", "");
        }

        public static IReadOnlyList<MetadataExtractor.Directory> GetImageMetadata(string imageName)
        {
            return ImageMetadataReader.ReadMetadata(FileLocationPath + imageName);
        }

        public static Image ReadImageFromFile(string imageName)
        {
            return Image.FromFile(FileLocationPath + imageName);
        }
    }
}
