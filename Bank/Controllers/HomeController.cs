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
            var all = db.GetAccounts();
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

        protected ActionResult
            Transfer(int from, int to, int amount, string note = "Web transfer")
        {
            if (to == from)
                return RedirectToAction("BadTransfer");
            Transaction t = new Transaction();
            t.To = AccountById(db, to);
            t.From = AccountById(db, from);
            t.Amount = amount;
            t.Note = note;
            db.GetTransactions().Add(t);
            return null;
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
            ActionResult r = Transfer(1, model.SelectedAccount, model.Amount);
            return r != null ? r :  RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Withdraw(MainMenuModel model)
        {
            if (AccountById(db, model.SelectedAccount).Locked)
                return RedirectToAction("BadWithdraw");
            ActionResult r = Transfer(model.SelectedAccount, 1, model.Amount);
            return r != null ? r : RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Transfer(MainMenuModel model)
        {
            ActionResult r =
                Transfer(model.FromAccount, model.ToAccount, model.Amount);
            return r != null ? r : RedirectToAction("MainMenu", model);
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


        public ActionResult BadTransfer()
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