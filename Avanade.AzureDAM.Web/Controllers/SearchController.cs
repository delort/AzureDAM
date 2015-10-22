using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Avanade.AzureDAM.Queries;

namespace Avanade.AzureDAM.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly QueryFactory _queryFactory;

        public SearchController(QueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public ActionResult Find(string searchCriteria)
        {
            var query = _queryFactory.CreateQuery<FindAssetQuery>();

            var result = query.For(searchCriteria)
                              .Load();

            return View("Results", result);
        }

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
    }
}