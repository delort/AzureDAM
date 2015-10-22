using System;
using Avanade.AzureDAM.Commands;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.Queries;
using Avanade.AzureDAM.Storage;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Avanade.AzureDAM.API.App_Start
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

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ImageFileRepository>();
            container.RegisterType<MessageBusClient>();
            container.RegisterType<CommandFactory>();
            container.RegisterType<QueryFactory>();
            container.RegisterType<UploadAssetNotifier>();
            container.RegisterType<AssetDocumentRepository>();
            container.RegisterType<AssetSearchRepository>();
            container.RegisterInstance<IUnityContainer>(container);

            // register decorators
            container.RegisterType<FindAssetQuery, FindAssetQueryManagedLinks>();
        }
    }
}
