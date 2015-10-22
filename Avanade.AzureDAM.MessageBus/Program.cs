using System.Diagnostics;
using Microsoft.Azure.WebJobs;

namespace Avanade.AzureDAM.MessageBus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Initialize();
            Run();
        }

        private static void Run()
        {
            //TODO: Add custom job activator
            var configuration = new JobHostConfiguration();
            new JobHost().RunAndBlock();
        }

        private static void Initialize()
        {
            var writer = new TextWriterTraceListener(System.Console.Out);
            Debug.Listeners.Add(writer);
            MessageFunctions.Init();
        }
    }
}
