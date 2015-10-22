using System;
using System.Linq;
using System.Runtime.InteropServices;
using Avanade.AzureDAM.MessageHandlers;
using Avanade.AzureDAM.Messages;
using Microsoft.Practices.Unity;

namespace Avanade.AzureDAM.MessageBus.Dispatcher
{
    public class MessageDispatch
    {
        private readonly IUnityContainer _container;
        private readonly DispatchConfiguration _configuration;

        public MessageDispatch(IUnityContainer container, DispatchConfiguration configuration)
        {
            _container = container;
            _configuration = configuration;
            _configuration.ConfigureHandlersFrom(typeof(IHandle<>).Assembly);
        }

        public void Dispatch<TMessage>(TMessage message) where TMessage:Message
        {
            _configuration.GetHandlersFor<TMessage>()
                          .AsParallel()
                          .ForAll(handler => Dispatch(handler, message));
        }

        private void Dispatch<T>(Type handler, T message) where T: Message
        {
            var handlerObj = (IHandle<T>) _container.Resolve(handler);
            handlerObj.Handle(message);
        }
    }
}