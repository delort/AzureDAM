using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Avanade.AzureDAM.MessageBus.Infrastructure;
using Avanade.AzureDAM.MessageHandlers;
using Microsoft.Practices.Unity;

namespace Avanade.AzureDAM.MessageBus.Dispatcher
{
    public class DispatchConfiguration
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<Type, List<Type>> _messageHandlers;

        public DispatchConfiguration(IUnityContainer container)
        {
            _container = container;
            _messageHandlers = new Dictionary<Type, List<Type>>();
        }

        public void ConfigureHandlersFrom(Assembly handlerAssembly)
        {
            Debug.WriteLine("Registering listeners:");

            var handlers = handlerAssembly.GetTypes()
                .Where(type => type.GetInterfaces()
                    .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IHandle<>)) != null)
                .ToList();

            foreach (var handlerType in handlers)
            {
                handlerType.GetInterfaces()
                      .Where(i => i.GetGenericTypeDefinition() == typeof(IHandle<>))
                      .Select(i => i.GetGenericArguments().First())
                      .ForEach(messageType => Add(messageType, handlerType));
            }

            Debug.WriteLine("Listener registration done.");
        }

        public void Add(Type messageType, Type handlerType)
        {
            if (!_messageHandlers.ContainsKey(messageType))
                _messageHandlers.Add(messageType, new List<Type>());

            _messageHandlers[messageType].Add((handlerType));
            _container.RegisterType(handlerType);

            Debug.WriteLine("Listener added: {0} is now listening to {1}", handlerType.Name, messageType.Name);
        }

        public IEnumerable<Type> GetHandlersFor<TMessage>()
        {
            return _messageHandlers[typeof(TMessage)];
        }
    }
}