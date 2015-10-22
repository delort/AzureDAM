using System;
using System.Diagnostics;
using Avanade.AzureDAM.DemoSetup.Search;

namespace Avanade.AzureDAM.DemoSetup
{
    class Program
    {
        static void Main(string[] args)
        {

            var writer = new TextWriterTraceListener(System.Console.Out);
            Debug.Listeners.Add(writer);

            //Console.WriteLine("Resetting Azure resources");
            //AssetCleanup.Run();
            //Console.WriteLine("Completed.");

            //Console.WriteLine("Importing assets");
            //AssetImport.Run();
            //Console.WriteLine("Completed.");

            //Console.WriteLine("Deleting search artifacts");
            //SearchCleanUp.Run();
            //Console.WriteLine("Completed.");

            Console.WriteLine("Creating search artifacts");
            SearchSetup.Run();
            Console.WriteLine("Completed.");

            Console.WriteLine("Setup done.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }

}
