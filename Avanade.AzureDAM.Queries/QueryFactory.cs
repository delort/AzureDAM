using Microsoft.Practices.Unity;

namespace Avanade.AzureDAM.Queries
{
    public class QueryFactory
    {
        private IUnityContainer _container;

        public QueryFactory(IUnityContainer container)
        {
            _container = container;
        }

        public T CreateQuery<T>() where T : IQuery
        {
            return _container.Resolve<T>();
        }
    }
}
