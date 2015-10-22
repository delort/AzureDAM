using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Avanade.AzureDAM.Integrations.Facades;
using Avanade.AzureDAM.Models;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace Avanade.AzureDAM.Integrations.Repositories
{
    public class MediaServicesRepository
    {
        private readonly CloudMediaContext _context;
        private readonly BlobStorageFacade _blobStorage;
        private readonly MediaServicesFacade _mediaServices;


        public MediaServicesRepository()
        {
            var accountName = ConfigurationManager.AppSettings["MediServicesName"];
            var accountKey = ConfigurationManager.AppSettings["MediServicesKey"];

            _context = new CloudMediaContext(accountName, accountKey);

            var blobStorageConnectionString = ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
            _blobStorage = new BlobStorageFacade(blobStorageConnectionString);

            _mediaServices = new MediaServicesFacade(_context);
        }

        public AssetStorageMetadata StoreAsset(string assetName, string videoName, string contentType, byte[] assetContents)
        {
            var asset = _context.Assets.Create(assetName, AssetCreationOptions.None);
            var assetFile = asset.AssetFiles.Create(videoName);
            assetFile.ContentFileSize = assetContents.Length;
            
            var uploadLocator = _mediaServices.CreateSasLocator(asset, _mediaServices.UploadPolicy);

            var destinationContainerName = (new Uri(uploadLocator.Path)).Segments[1];

            _blobStorage.AddFileToBlobStorage(destinationContainerName, videoName, contentType, assetContents);
            assetFile.Update();

            _mediaServices.UploadPolicy.Delete();

            var publicLocator = _mediaServices.CreateSasLocator(asset, _mediaServices.PublicPolicy);

            var metadata = new AssetStorageMetadata
            {
                Id = assetName,
                StorageId = asset.Id, 
                StorageLocation = publicLocator.Path
            };

            return metadata;
        }

        
        public IAsset GetAsset(string assetId)
        {
            // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
            return _context.Assets.Where(a => a.Id == assetId).FirstOrDefault();
        }
        
        public ILocator GetStreamingLocator(IAsset asset)
        {
            return _mediaServices.CreateOriginLocator(asset);
        }

        public IJob CreateEncodingJob(IAsset asset, string preset)
        {
            return CreateJob(asset, preset, "Media Encoder", "Encode streaming");
        }

        public IJob CreateMediaIndexingJob(IAsset asset, string configuration)
        {
            return CreateJob(asset, configuration, "Azure Media Indexer", "Media index");
        }

        public IJob CreateJob(IAsset asset, string configuration, string processorName, string action)
        {
            var name = $"{asset.Name.Substring(0,8)} - {action}";
            var mediaProcessor = GetMediaProcessor(processorName);

            var job = _context.Jobs.Create(name + " job");
            var task = job.Tasks.AddNew(name + " task",
                                        mediaProcessor,
                                        configuration,
                                        TaskOptions.ProtectedConfiguration);

            task.InputAssets.Add(asset);
            task.OutputAssets.AddNew(name,
                                     AssetCreationOptions.None);

            return job;
        }

        private IMediaProcessor GetMediaProcessor(string processorName)
        {
            var mediaProcessor = _context.MediaProcessors
                .Where(processor => processor.Name.Contains(processorName))
                .ToList()
                .OrderByDescending(processor => new Version(processor.Version))
                .FirstOrDefault();
            return mediaProcessor;
        }

       
    }
}