using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bank.Database;
using System.Configuration;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        IBankDbContext db;

        public ActionResult Index()
        {
            var model = new HomeViewModel();
            model.SetUserOptions(db);
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(HomeViewModel model)
        {
            if (model.UserName == null || model.UserName == "0")
            {
                var homeViewModel = new HomeViewModel();
                homeViewModel.SetUserOptions(db);
                return View("Index", homeViewModel);
            }
            MainMenuModel m = new MainMenuModel();
            var usernameByIndex =
                db.GetUsers().ToDictionary(g => g.ID.ToString(), g => g.Name);
            m.UserName =  usernameByIndex[model.UserName];
            m.UserID = int.Parse(model.UserName);
            return RedirectToAction("MainMenu", new RouteValueDictionary(m));
        }


        public ActionResult MainMenu(MainMenuModel model)
        {
            return View(model);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


        public HomeController(IBankDbContext DbCtx)
        {
            db = DbCtx;
        }
    }


    public class BankControllerFactory : DefaultControllerFactory
    {
        private Dictionary<string, Func<RequestContext, IController>>
            controllers;


        public BankControllerFactory(IBankDbContext repository)
        {
            controllers =
                new Dictionary<string, Func<RequestContext, IController>>();
            controllers["Home"] = controller => new HomeController(repository);
        }


        public override IController
        CreateController(RequestContext requestContext, string controllerName)
        {
            if (! controllers.ContainsKey(controllerName)) return null;

            return controllers[controllerName](requestContext);
        }


        public static IControllerFactory GetControllerFactory()
        {
            string typeName = ConfigurationManager.AppSettings["repository"];
            var type  = Type.GetType(typeName);
            var repository = Activator.CreateInstance(type);
            return new BankControllerFactory(repository as IBankDbContext);
        }
    }
}