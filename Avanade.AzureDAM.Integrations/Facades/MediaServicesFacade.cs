using System;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace Avanade.AzureDAM.Integrations.Facades
{
    public class MediaServicesFacade
    {
        private readonly CloudMediaContext _context;

        public MediaServicesFacade(CloudMediaContext context)
        {
            _context = context;
        }

        public IAccessPolicy PublicPolicy => _context.AccessPolicies.Create("Public Policy", TimeSpan.FromDays(365), AccessPermissions.Read);

        public IAccessPolicy StreamingPolicy => _context.AccessPolicies.Create("Streaming Policy", TimeSpan.FromDays(30), AccessPermissions.Read);

        public IAccessPolicy UploadPolicy => _context.AccessPolicies.Create("uploadPolicy", TimeSpan.FromHours(1), AccessPermissions.Write);

        public ILocator CreateOriginLocator(IAsset asset)
        {
           return _context.Locators.CreateLocator(LocatorType.OnDemandOrigin, asset, StreamingPolicy, DateTime.UtcNow.AddMinutes(-5));
        }

        public ILocator CreateSasLocator(IAsset asset, IAccessPolicy policy)
        {
            return _context.Locators.CreateLocator(LocatorType.Sas, asset, StreamingPolicy);
        }
    }
}