using System.Drawing;
using System.IO;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Models;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace Avanade.AzureDAM.MessageHandlers
{
    internal class GenerateThumbnail : IHandle<NewImageAdded>
    {
        private readonly ImageFileRepository _fileRepository;
        private readonly AssetDocumentRepository _documentRepository;

        public GenerateThumbnail(ImageFileRepository fileRepository, AssetDocumentRepository documentRepository)
        {
            _fileRepository = fileRepository;
            _documentRepository = documentRepository;
        }

        public void Handle(NewImageAdded message)
        {
            var originalAsset = _fileRepository.ReadImage(message.ImageName);

            var thumbnailLocation = Generate(originalAsset);
            AddDerivate(message, thumbnailLocation);

        }

        private string Generate(StoredAsset originalAsset)
        {
            using (var inputStream = originalAsset.FileStream)
            using (var outputStream = new MemoryStream())
            using (var factory = new ImageFactory())
            {
                factory.Load(inputStream)
                    .Resize(new Size(0, 150))
                    .Format(new JpegFormat() {Quality = 70})
                    .Save(outputStream);

                var outputBytes = new byte[outputStream.Length];
                outputStream.Read(outputBytes, 0, (int) outputStream.Length);
                var thumbnail = _fileRepository.StoreAsset(originalAsset.Id, "thumbnail" + originalAsset.Extension, originalAsset.ContentType,
                    outputBytes);

                return thumbnail.StorageLocation;
            }
        }

        private void AddDerivate(NewImageAdded message, string thumbnailLocation)
        {
            var metadata = _documentRepository.Get(message.Id);

            metadata.Published = true;
            var newDerivate = new Derivate
            {
                Name = "Thumbnail",
                Url = thumbnailLocation
            };

            metadata.Derivates.Add(newDerivate);
            _documentRepository.Update(metadata);
        }
    }
}