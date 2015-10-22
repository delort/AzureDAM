using System.Web.Mvc;

namespace Avanade.AzureDAM.API.ActionFilters
{
    public class RequireAPIKeyAttribute : ActionFilterAttribute
    {
        private const string APIKey = "api-key";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var headers = filterContext.HttpContext.Request.Headers;

            var apiKey = headers[APIKey];

            if(string.IsNullOrEmpty(apiKey))
                filterContext.Result = new HttpUnauthorizedResult("Required api-key header not provided");

        }
    }
    
}