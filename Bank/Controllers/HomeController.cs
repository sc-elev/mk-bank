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
            m.UserName = usernameByIndex[model.UserName];
            m.UserID = int.Parse(model.UserName);
            var dict = db.GetAccounts().ToDictionary(g => g.ID, g => g.Name);
            var AccountNames = new SelectList(dict, "key", "value");
            ViewBag.data = AccountNames;
            return RedirectToAction("MainMenu", "", m);
        }



        public ActionResult MainMenu(MainMenuModel model)
        {
            var dict = db.GetAccounts().ToDictionary(g => g.ID, g => g.Name);
            var AccountNames = new SelectList(dict, "key", "value");
            ViewBag.data = AccountNames;
            return View("MainMenu", "", model);
        }


        [HttpPost]
        public ActionResult ListAccounts(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            var found = accounts.Where(a => a.User_ID == model.UserID).ToList();
            return View("ListAccounts", found);
        }


        [HttpPost]
        public ActionResult ListBalances(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            var found = accounts.Where(a => a.User_ID == model.UserID).ToList();
            return View("ListBalances", found);
        }


        [HttpPost]
        public ActionResult ListBalance(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            var found =
                accounts.Where(a => a.ID == model.SelectedAccount).Single();
            return View("ListBalance", found);
        }


        [HttpPost]
        public ActionResult ListTransactions(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            Account account  =
                accounts.Where(a => a.ID == model.SelectedAccount).Single();
            return View("ListTransactions", account.Transactions);
        }


        [HttpPost]
        public ActionResult AddMoney(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            var account =
                accounts.Where(a => a.ID == model.SelectedAccount).Single();
            Transaction t = new Transaction();
            t.From = accounts[0];
            t.To = account;
            t.Amount = model.Amount;
            t.Note = "Manual insert";
            db.GetTransactions().Add(t);
            return RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Withdraw(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            var account =
                accounts.Where(a => a.ID == model.SelectedAccount).Single();
            Transaction t = new Transaction();
            t.From = account;
            t.To = accounts[0];
            t.Amount = model.Amount;
            t.Note = "Manual withdrawal";
            db.GetTransactions().Add(t);
            return RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Transfer(MainMenuModel model)
        {
            Transaction t = new Transaction();
            t.To =
                db.GetAccounts().Where(a => a.ID == model.ToAccount).Single();
            t.From =
                db.GetAccounts().Where(a => a.ID == model.FromAccount).Single();
            t.Amount = model.Amount;
            t.Note = "Web Transfer";
            db.GetTransactions().Add(t);
            return RedirectToAction("MainMenu", model);
        }


        [HttpPost]
        public ActionResult Lock(MainMenuModel model)
        {
            IList<Account> accounts = db.GetAccounts();
            var found = accounts.Where(a => a.ID == model.SelectedAccount).Single();
            found.Locked = true;
            return RedirectToAction("MainMenu", model);
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