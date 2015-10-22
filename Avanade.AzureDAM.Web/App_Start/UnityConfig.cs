using System;
using Avanade.AzureDAM.Commands;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Integrations.Decorators;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Queries;
using Avanade.AzureDAM.Storage;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Avanade.AzureDAM.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<MessageBusClient>();

            container.RegisterType<CommandFactory>();
            container.RegisterType<QueryFactory>();

            container.RegisterType<UploadAssetNotifier>();

            container.RegisterType<ImageFileRepository>();
            container.RegisterType<AssetDocumentRepository>();
            container.RegisterType<IAssetDocumentRepository, AssetDocumentCDNDecorator>();
            container.RegisterType<AssetSearchRepository>();
            

            container.RegisterInstance<IUnityContainer>(container);

            container.RegisterType<UploadCommand, UploadImageCommand>(AssetTypeMapping.Image);
            container.RegisterType<UploadCommand, UploadVideoCommand>(AssetTypeMapping.Video);
        }
    }

    
}
