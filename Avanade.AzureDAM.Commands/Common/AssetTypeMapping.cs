using System;
using System.Collections.Generic;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Models;

namespace Avanade.AzureDAM.Commands
{
    public class AssetTypeMapping
    {
        public const string Image = "image";
        public const string Video = "video";

        private static readonly Dictionary<string, string> _contentTypeMapping = new Dictionary<string, string>()
        {
            {"video/mp4", Video},
            {"video/x-ms-wmv", Video},
            { "image/jpg", Image},
            { "image/jpeg", Image}
        };
        
        public static string GetTypeFor(string extension) => _contentTypeMapping[extension.ToLower()];
    }
}