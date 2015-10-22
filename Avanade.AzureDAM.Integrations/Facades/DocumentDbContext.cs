using System;
using System.Configuration;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Avanade.AzureDAM.Integrations.Facades
{
    public class DocumentDbContext : IDisposable
    {
        private readonly string _endPoint;
        private readonly string _athorizationKey;
        private DocumentClient _client;
        private Database _database;

        public DocumentDbContext()
        {
            _endPoint = ConfigurationManager.AppSettings["DocumentDBEndpoint"];
            _athorizationKey = ConfigurationManager.AppSettings["DocumentDBAuthorizationKey"];
            _client = CreateNewClient();
        }

        private DocumentClient CreateNewClient()
        {
            return new DocumentClient(new Uri(_endPoint), _athorizationKey);
        }

        public void OpenDatabase(string databaseName)
        {
            _database = _client.CreateDatabaseQuery()
                               .Where(db => db.Id == databaseName)
                               .ToArray()
                               .FirstOrDefault();

            if (_database == null)
                throw new ApplicationException($"Could not find database {databaseName}");
        }

        public DocumentCollection GetCollection(string collectionName)
        {
            var collection = _client.CreateDocumentCollectionQuery(_database.SelfLink)
                                       .Where(col => col.Id == collectionName)
                                       .ToArray()
                                       .FirstOrDefault();

            if (collection == null)
                throw new ApplicationException($"Could not find collection {collectionName}");

            return collection;
        }

        public void UpdateDocument(string id, object updatedDocument, string collectionName)
        {
            var originalDocument = CreateDocumentQuery<Document>(collectionName)
                                .Where(document => document.Id == id)
                                .AsEnumerable().FirstOrDefault();

            if (originalDocument != null)
                _client.ReplaceDocumentAsync(originalDocument.SelfLink, updatedDocument)
                    .Wait();
        }

        public void CreateDocument(object document, string collectionName)
        {
            var collection = GetCollection(collectionName);
            _client.CreateDocumentAsync(collection.SelfLink, document)
                   .Wait();

        }

        public IOrderedQueryable<ModelT> CreateDocumentQuery<ModelT>(string collectionName)
        {
            var collection = GetCollection(collectionName);

            return _client.CreateDocumentQuery<ModelT>(collection.SelfLink);
        }

        public void DeleteCollection(string collectionName)
        {
            var collectionToDelete = GetCollection(collectionName);
            _client.DeleteDocumentCollectionAsync(collectionToDelete.SelfLink)
                   .Wait();
        }

        public void CreateCollection(string collectionName)
        {
            _client.CreateDocumentCollectionAsync("dbs/" + _database.Id,
                                                     new DocumentCollection
                                                     {
                                                         Id = collectionName
                                                     },
                                                     new RequestOptions { OfferType = "S1" })
                                                     .Wait();
        }

        private bool _isDisposed;
        public void Dispose()
        {
            _client.Dispose();
            _isDisposed = true;
        }

        ~DocumentDbContext()
        {
            if (!_isDisposed)
                this.Dispose();
        }
    }
}
