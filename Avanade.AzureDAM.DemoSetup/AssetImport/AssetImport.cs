using Avanade.AzureDAM.Models;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Messages;

namespace Avanade.AzureDAM.DemoSetup
{
    public class AssetImport
    {
        public static void Run()
        {
            var files = FileReader.ReadMetadataFile();
            var attributes = new Dictionary<string, List<string>>();

            foreach (var file in files.Take(50))
            {
                if (file.FileName.Contains(".pptx"))
                    continue;

                //Console.WriteLine("Uploading: {0}", file.FileName);
                UploadAsset(file);
                //Console.WriteLine("Done.");
                Console.Write(".");
            }
            Console.WriteLine();
        }

        private static void UploadAsset(FileMetadata file)
        {
            var assetFileRepository = new ImageFileRepository();
            var assetDocumentRepository = new AssetDocumentRepository();
            var notifier = new MessageBusClient();

            var assetRootPath = ConfigurationManager.AppSettings["ImageRootPath"];

            var id = Guid.NewGuid().ToString();
            var fileInfo = FileReader.GetFileInfo(file.FileLocation);
            var fileContents = FileReader.ReadFile(file.FileLocation);
            var fileName = $"original{fileInfo.Extension}";
                

            var storageMetadata = assetFileRepository.StoreAsset(id, fileName, "image/jpeg", fileContents);

            if (storageMetadata == null)
                return;

            var imageMetadata = FileReader.GetImageMetadata(file.FileName);
            var imageDescription = imageMetadata.GetValue("Exif IFD0", "Image Description");


            var assetMetadata = new AssetMetadata
            {
                Id = id,
                Name = file.FileName,
                Description = imageDescription ?? string.Empty,
                Category = file.Category,
                ContentType = "image/jpeg",
                Author = file.Photographer,
                Url = $"{assetRootPath}/{storageMetadata.StorageId}",
                StorageMetadata = storageMetadata,
                Type = "image"
            };

            assetDocumentRepository.Create(assetMetadata);

            notifier.SendMessage(new NewImageAdded { ImageName = storageMetadata.StorageId, Id = id});
        }
    }
}
