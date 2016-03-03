using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bank.Database;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        IBankDbContext db = new BankDbContext();

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
                return View(new HomeViewModel());
            MainMenuModel m = new MainMenuModel();
            var usernameByIndex =
                db.GetUsers().ToDictionary(g => g.ID.ToString(), g => g.Name);
            m.UserName =  usernameByIndex[model.UserName];
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
    }
}