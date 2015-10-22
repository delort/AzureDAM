using Avanade.AzureDAM.Commands;
using Avanade.AzureDAM.Models;
using Avanade.AzureDAM.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avanade.AzureDAM.Web.Controllers
{
    public class AssetsController : Controller
    {
        private readonly CommandFactory _commandFactory;
        private readonly QueryFactory _queryFactory;

        public AssetsController(CommandFactory commandFactory, QueryFactory queryFactory)
        {
            _commandFactory = commandFactory;
            _queryFactory = queryFactory;
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var model = _queryFactory.CreateQuery<GetAssetDetailsQuery>()
                                     .For(id)
                                     .Load();

            return View("display", model);
        }

        // GET: AssetManagement
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View("Upload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(NewAssetViewModel model, HttpPostedFileBase assetToUpload)
        {
            var assetType = AssetTypeMapping.GetTypeFor(assetToUpload.ContentType);
            var command = _commandFactory.CreateCommand<UploadCommand>(assetType);

            var result = command.With(model.Name, model.Description, assetToUpload)
                   .Do();

            return RedirectToAction("Details", new { id = result.Id });
        }
    }
}