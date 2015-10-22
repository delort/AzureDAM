using System.Linq;
using System.Threading;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Models;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace Avanade.AzureDAM.MessageHandlers.NewVideoAddedHandlers
{
    public class EncodeSmoothStreamingVideo : IHandle<NewVideoAdded>
    {
        private const string AdaptiveBitRateConfiguration = "H264 Adaptive Bitrate MP4 Set 720p";

        private readonly MediaServicesRepository _mediaServicesRepository;
        private readonly AssetDocumentRepository _documentRepository;
        private readonly MessageBusClient _messageBusClient;


        public EncodeSmoothStreamingVideo(MediaServicesRepository mediaServicesRepository, AssetDocumentRepository documentRepository, MessageBusClient messageBusClient)
        {
            _mediaServicesRepository = mediaServicesRepository;
            _documentRepository = documentRepository;
            _messageBusClient = messageBusClient;
        }

        public void Handle(NewVideoAdded message)
        {
            var streamingLocation = ExecuteEncodingJob(message);

            if (streamingLocation == string.Empty)
                return;

            UpdateMetadata(message, streamingLocation);
        }

        private void UpdateMetadata(NewVideoAdded message, string streamingLocation)
        {
            var metadata = _documentRepository.Get(message.Id);

            metadata.Published = true;
            var newDerivate = new Derivate
            {
                Name = AdaptiveBitRateConfiguration,
                Url = streamingLocation
            };

            metadata.Derivates.Add(newDerivate);
            _documentRepository.Update(metadata);
        }

        private string ExecuteEncodingJob(NewVideoAdded message)
        {
            var asset = _mediaServicesRepository.GetAsset(message.AssetId);

            if (asset == null)
                return string.Empty;

            var job = _mediaServicesRepository.CreateEncodingJob(asset, AdaptiveBitRateConfiguration);
            ReactOnStatusChange(message, job, asset);

            job.Submit();
            job.GetExecutionProgressTask(CancellationToken.None)
               .Wait();

            var streamingAsset = job.OutputMediaAssets[0];
            var originLocator = _mediaServicesRepository.GetStreamingLocator(streamingAsset);

            var manifestFile = streamingAsset.AssetFiles
                                             .Where(file => file.Name.EndsWith(".ism"))
                                             .First();

            return originLocator.Path + manifestFile.Name;
        }

        private void ReactOnStatusChange(NewVideoAdded message, IJob job, IAsset asset)
        {
            job.StateChanged += (sender, args) =>
            {
                switch (args.CurrentState)
                {
                    case JobState.Error:
                        _messageBusClient.SendMessage(new EncodingError {JobId = job.Id});
                        break;
                    case JobState.Finished:
                        _messageBusClient.SendMessage(new EncodingCompleted
                        {
                            AssetId = asset.Id,
                            Id = message.Id,
                            JobId = job.Id
                        });
                        break;
                }
            };
        }
    }
}