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

        protected Account AccountById(IBankDbContext db, int ID)
        {
            return db.GetAccounts().Where(a => a.ID == ID).Single();
        }

        protected IList<Account> AccountsByUser(int UserID)
        {
            return db.GetAccounts().Where(a => a.User_ID == UserID).ToList();
        }

        protected SelectList AccountNamesSelectList(IBankDbContext db)
        {
            var dict = db.GetAccounts().ToDictionary(g => g.ID, g => g.Name);
            return new SelectList(dict, "key", "value");
        }

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
            m.UserName = db.GetUsers()
                .Where(u => u.ID.ToString() == model.UserName)
                .Single()
                .Name;
            m.UserID = int.Parse(model.UserName);
            ViewBag.data = AccountNamesSelectList(db);
            return RedirectToAction("MainMenu", "", m);
        }


        public ActionResult MainMenu(MainMenuModel model)
        {
            ViewBag.data = AccountNamesSelectList(db);
            return View("MainMenu", "", model);
        }


        [HttpPost]
        public ActionResult ListAccounts(MainMenuModel model)
        {
            return View("ListAccounts", AccountsByUser(model.UserID));
        }


        [HttpPost]
        public ActionResult ListBalances(MainMenuModel model)
        {
            return View("ListBalances", AccountsByUser(model.UserID));
        }


        [HttpPost]
        public ActionResult ListBalance(MainMenuModel model)
        {
            return View("ListBalance", AccountById(db, model.SelectedAccount));
        }


        [HttpPost]
        public ActionResult ListTransactions(MainMenuModel model)
        {
            var account = AccountById(db, model.SelectedAccount);
            return View("ListTransactions", account.Transactions);
        }


        [HttpPost]
        public ActionResult AddMoney(MainMenuModel model)
        {
            Transaction t = new Transaction();
            t.From = AccountById(db, 1);
            t.To =  AccountById(db, model.SelectedAccount);
            t.Amount = model.Amount;
            t.Note = "Web insert";
            db.GetTransactions().Add(t);
            return RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Withdraw(MainMenuModel model)
        {
            Transaction t = new Transaction();
            t.From = AccountById(db, model.SelectedAccount);
            if (t.From.Locked)
                return RedirectToAction("BadWithdraw");
            t.To = AccountById(db, 1);
            t.Amount = model.Amount;
            t.Note = "Manual withdrawal";
            db.GetTransactions().Add(t);
            return RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Transfer(MainMenuModel model)
        {
            Transaction t = new Transaction();
            t.To = AccountById(db, model.ToAccount);
            t.From = AccountById(db, model.FromAccount);
            t.Amount = model.Amount;
            t.Note = "Web Transfer";
            db.GetTransactions().Add(t);
            return RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Lock(MainMenuModel model)
        {
            AccountById(db, model.SelectedAccount).Locked = true;
            return RedirectToAction("MainMenu", model);
        }


        public ActionResult BadWithdraw()
        {
            return View();
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
}