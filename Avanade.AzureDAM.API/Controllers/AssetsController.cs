using System.Web;
using System.Web.Mvc;
using Avanade.AzureDAM.API.ActionFilters;
using Avanade.AzureDAM.Integrations.Clients;
using Avanade.AzureDAM.Messages;
using Avanade.AzureDAM.Queries;

namespace Avanade.AzureDAM.API.Controllers
{
    public class AssetsController : Controller
    {
        private readonly QueryFactory _queryFactory;
        private readonly MessageBusClient _messageClient;

        public AssetsController(QueryFactory queryFactory, MessageBusClient messageClient)
        {
            _queryFactory = queryFactory;
            _messageClient = messageClient;
        }


        public FileResult Get(string channel, string id)
        {
            var query = _queryFactory.CreateQuery<GetAssetFileQuery>();

            var result = query.For(channel, id)
                              .Load();

            if (result != null)
                return new FileStreamResult(result.FileStream, result.ContentType);

            //TODO: Should be part of the demo.
            _messageClient.SendMessage(new ChannelAssetError
            {
                AssetName   = id, 
                ChannelName = channel, 
                Reason = "Not found"
            });

            throw new HttpException(404, $"Could not find {id}");
        }           

        [AllowJsonGet]
        [RequireAPIKey]
        public JsonResult Find(string queryString)
        {
            var query = _queryFactory.CreateQuery<FindAssetQuery>();

            var result = query.For(queryString)
                              .Load();

            return new JsonResult {Data = result};
        }
    }
}