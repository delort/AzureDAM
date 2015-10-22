using System;
using System.Collections.Generic;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Commands
{
    public class UploadAssetNotifier
    {
        private Dictionary<string, Action<AssetStorageMetadata>> _messageTypeMapping => new Dictionary
          <string, Action<AssetStorageMetadata>>
        {
            {
                "image", (metadata) =>
                {
                    _client.SendMessage(new NewImageAdded()
                    {
                        Id = metadata.Id,
                        ImageName = metadata.StorageId
                    });
                }
            },
             {
                "video", (metadata) =>
                {
                    _client.SendMessage(new NewVideoAdded()
                    {
                        Id = metadata.Id,
                        AssetId = metadata.StorageId
                    });
                }
            }
        };


        private readonly MessageBusClient _client;

        public UploadAssetNotifier(MessageBusClient messageBusClient)
        {
            _client = messageBusClient;
        }

        public void SendNotifications(AssetMetadata metadata) => _messageTypeMapping[metadata.Type]?.Invoke((metadata.StorageMetadata));

    }
}