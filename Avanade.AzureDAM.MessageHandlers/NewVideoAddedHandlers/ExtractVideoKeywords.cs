using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Models;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace Avanade.AzureDAM.MessageHandlers.NewVideoAddedHandlers
{
    //TODO: Cleanup index asset
    public class ExtractVideoKeywords : IHandle<NewVideoAdded>
    {

        private readonly MediaServicesRepository _mediaServicesRepository;
        private readonly AssetDocumentRepository _documentRepository;
        private readonly MessageBusClient _messageBusClient;


        public ExtractVideoKeywords(MediaServicesRepository mediaServicesRepository, AssetDocumentRepository documentRepository, MessageBusClient messageBusClient)
        {
            _mediaServicesRepository = mediaServicesRepository;
            _documentRepository = documentRepository;
            _messageBusClient = messageBusClient;
        }

        public void Handle(NewVideoAdded message)
        {
            var job = ExecuteIndexingJob(message);

            if (job == null)
                return;

            var keywords = ReadKeywords(job);

            UpdateMetadata(message, keywords);

        }

        private IJob ExecuteIndexingJob(NewVideoAdded message)
        {
            var asset = _mediaServicesRepository.GetAsset(message.AssetId);

            if (asset == null)
                return null;

            var job = _mediaServicesRepository.CreateMediaIndexingJob(asset, indexingConfiguration.Replace("$language$", "English"));
            job.Submit();

            job.GetExecutionProgressTask(CancellationToken.None)
                .Wait();

            if (job.State == JobState.Error)
            {
                throw new ApplicationException();
            }

            return job;
        }

        private static string ReadKeywords(IJob job)
        {
            var indexAsset = job.OutputMediaAssets[0];
            var keywordFile = indexAsset.AssetFiles
                .Where(file => file.Name.EndsWith(".kw.xml"))
                .First();

            var keyWordFileLocation = Path.Combine(Path.GetTempPath(), keywordFile.Name);
            keywordFile.Download(keyWordFileLocation);

            var keyWordDocument = new XmlDocument();
            keyWordDocument.Load(keyWordFileLocation);
            File.Delete(keyWordFileLocation);

            var keywords = keyWordDocument.GetElementsByTagName("mavis:keywords")[0]?.InnerText;

            return keywords;
        }

        private void UpdateMetadata(NewVideoAdded message, string keywords)
        {
            var metadata = _documentRepository.Get(message.Id);

            metadata.Keywords = keywords;

            _documentRepository.Update(metadata);
        }
        #region Index Configuration
        private string indexingConfiguration = $@"<?xml version=""1.0"" encoding=""utf-8""?>
        <configuration version=""2.0"">
          <input>
          </input>
          <settings>
          </settings>
          <features>
            <feature name=""ASR"" >
              <settings>
                <add key=""Language"" value=""$language$""/>
                <add key=""CaptionFormats"" value=""ttml""/>
                <add key=""GenerateAIB"" value=""false"" />
                <add key=""GenerateKeywords"" value=""true"" />
              </settings>
            </feature>
          </features>
        </configuration>";
        #endregion
    }
}