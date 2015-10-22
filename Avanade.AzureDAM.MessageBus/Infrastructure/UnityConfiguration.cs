using System;
using Avanade.AzureDAM.Integrations.Repositories;
using Avanade.AzureDAM.MessageBus.Dispatcher;
using Microsoft.Practices.Unity;

namespace Avanade.AzureDAM.MessageBus.Infrastructure
{
    public class UnityConfiguration
    {
        private static readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ImageFileRepository>();
            container.RegisterType<AssetDocumentRepository>();

            container.RegisterType<DispatchConfiguration>();
            container.RegisterType<MessageDispatch>();

            container.RegisterInstance<IUnityContainer>(container);
        }
    }
}