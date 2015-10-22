using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanade.AzureDAM.Queries
{
    public interface IQuery {}

    public abstract class Query<ModelT> : IQuery
    {
        public abstract ModelT Load();
    }
}
