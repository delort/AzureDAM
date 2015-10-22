using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avanade.AzureDAM.Commands
{
    public interface ICommand { }

    public interface Command<out TCommandResult> : ICommand where TCommandResult : CommandResult 
    {
        TCommandResult Do();
    }
}
