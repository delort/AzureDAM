using System;
using System.Configuration;
using System.Threading;

using Avanade.AzureDAM.Integrations.Facades;

namespace Avanade.AzureDAM.DemoSetup
{ 
    public class AssetCleanup
    {
        private static BlobStorageFacade _blobStorage;
        private static string _containerName;
        private static string _databaseName;
        private static string _collectionName;

        private static void Setup()
        {
            var blobStorageConnectionString = ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;

            _containerName = ConfigurationManager.AppSettings["ImagesContainer"];
            _blobStorage = new BlobStorageFacade(blobStorageConnectionString);
            _databaseName = ConfigurationManager.AppSettings["DocumentDBDatabaseName"];
            _collectionName = ConfigurationManager.AppSettings["AssetCollectionName"];
        }

        public static void Run()
        {
            Setup();
            Delete();
            Console.WriteLine("Pausing 60s awaiting delete and release");
            Thread.Sleep(60000);
            Create();
        }

        private static void Create()
        {

            using (var dbContext = new DocumentDbContext())
            {
                dbContext.OpenDatabase(_databaseName);
                Console.WriteLine("Recreating db collection");
                dbContext.CreateCollection(_collectionName);
            }

            Console.WriteLine("Recreating blob storage");
            _blobStorage.GetBlobContainer(_containerName);

        }

        private static void Delete()
        {
            Console.WriteLine("Deleting blob storage");
            _blobStorage.DeleteContainer(_containerName);
            
            using (var dbContext = new DocumentDbContext())
            {
                dbContext.OpenDatabase(_databaseName);
                Console.WriteLine("Deleting db collection");
                try
                {
                    dbContext.DeleteCollection(_collectionName);
                }
                catch (ApplicationException){}
            }
            
        }
    }
}
