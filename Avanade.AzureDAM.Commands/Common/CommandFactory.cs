using Microsoft.Practices.Unity;

namespace Avanade.AzureDAM.Commands
{
    public class CommandFactory
    {
        private IUnityContainer _container;

        public CommandFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T CreateCommand<T>() where T : ICommand
        {
            return _container.Resolve<T>();
        }

        public T CreateCommand<T>(string variant) where T : ICommand
        {
            return _container.Resolve<T>(variant);
        }
    }
}
