using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank.Controllers;
using Bank.Database;
using Bank.Models;

namespace Bank.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller =
                new HomeController(new DatabaseMockup());

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller =
                new HomeController(new DatabaseMockup());

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.",
                            result.ViewBag.Message);
        }


        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller =
                new HomeController(new DatabaseMockup());

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void TestIndex()
        // Index invoked without username: return to /index page with set Model
        {
            var controller = new HomeController(new DatabaseMockup());
            var model = new HomeViewModel();

            var result = controller.Index(model) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
        }


        [TestMethod]
        public void TestIndexWithName0()
        // Index invoked with username == 0:  return to /index page.
        {
            var controller = new HomeController(new DatabaseMockup());
            var model = new HomeViewModel();
            model.UserName = "0";

            var result = controller.Index(model) as ViewResult;

            Assert.AreEqual("Index", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
        }


        [TestMethod]
        public void TestIndexWithUser()
        // Index called with user: invoke MainMenu with UserName set.
        {
            var controller = new HomeController(new DatabaseMockup());
            var model = new HomeViewModel();
            model.UserName = "1";
            model.SetUserOptions(new DatabaseMockup());

            var result = controller.Index(model) as RedirectToRouteResult;
            var values =
                result.RouteValues.ToDictionary(g => g.Key, g => g.Value);

            Assert.AreEqual("MainMenu", values["action"]);
            Assert.AreEqual("Orvar Slusk", values["UserName"]);
            Assert.AreEqual(1, values["UserID"]);
        }

        [TestMethod]
        public void TestListAccounts()
        // List all acounts for given user.
        {
            var controller = new HomeController(new DatabaseMockup());
            var mainModel = new MainMenuModel();
            mainModel.UserID = 1;
            mainModel.UserName = "Orvar Slusk";

            var result = controller.ListAccounts(mainModel) as ViewResult;

            Assert.AreEqual("ListAccounts", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsNotNull(result.ViewData.Model as IEnumerable<Account>);
            var accounts = (result.ViewData.Model as IEnumerable<Account>).ToList();
            Assert.AreEqual(6, accounts.Count);
        }

        [TestMethod]
        public void TestListBalances()
        // List all acounts  and their balance for given user.
        {
            var controller = new HomeController(new DatabaseMockup());
            var mainModel = new MainMenuModel();
            mainModel.UserID = 1;
            mainModel.UserName = "Orvar Slusk";

            var result = controller.ListBalances(mainModel) as ViewResult;

            Assert.AreEqual("ListBalances", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsNotNull(result.ViewData.Model as IEnumerable<Account>);
            var accounts = (result.ViewData.Model as IEnumerable<Account>).ToList();
            Assert.AreEqual(6, accounts.Count);
        }




        [TestMethod]
        public void TestAddMoney()
        // List all acounts  and their balance for given user.
        {
            var db = new DatabaseMockup();
            var controller = new HomeController(db);
            var model = new MainMenuModel();
            model.UserID = 1;
            model.UserName = "Orvar Slusk";
            model.SelectedAccount = 1;
            model.Amount = 12;
            var before = db.GetAccounts()[1].Balance;

            var result = controller.AddMoney(model) as ViewResult;

            Assert.AreEqual("AddMoney", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsNotNull(result.ViewData.Model as Account);
            Assert.AreEqual(before + 12, db.GetAccounts()[1].Balance);
        }


        [TestMethod]
        public void TestListTransactions()
        // List all transactions for given account.
        {
            var controller = new HomeController(new DatabaseMockup());
            var mainModel = new MainMenuModel();
            mainModel.UserID = 1;
            mainModel.UserName = "Orvar Slusk";
            mainModel.SelectedAccount = 1;

            var result = controller.ListTransactions(mainModel) as ViewResult;

            Assert.AreEqual("ListTransactions", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsNotNull(result.ViewData.Model as IEnumerable<Transaction>);
        }


        [TestMethod]
        public void TestWithdraw()
        // List all acounts  and their balance for given user.
        {
            var db = new DatabaseMockup();
            var controller = new HomeController(db);
            var model = new MainMenuModel();
            model.UserID = 1;
            model.UserName = "Orvar Slusk";
            model.SelectedAccount = 1;
            model.Amount = 12;
            var before = db.GetAccounts()[1].Balance;

            var result = controller.Withdraw(model) as ViewResult;

            Assert.AreEqual("Withdraw", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.IsNotNull(result.ViewData.Model as Account);
            Assert.AreEqual(before - 12, db.GetAccounts()[1].Balance);
        }


        [TestMethod]
        public void TestTransfer()
        // List all acounts  and their balance for given user.
        {
            var db = new DatabaseMockup();
            var controller = new HomeController(db);
            var model = new MainMenuModel();
            model.UserID = 1;
            model.UserName = "Orvar Slusk";
            model.FromAccount = 1;
            model.ToAccount = 4;
            model.Amount = 12;
            var srcBefore = db.GetAccounts()[1].Balance;
            var destBefore = db.GetAccounts()[4].Balance;

            var result = controller.Transfer(model) as ViewResult;

            Assert.AreEqual("Transfer", result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.AreEqual(srcBefore - 12, db.GetAccounts()[1].Balance);
            Assert.AreEqual(destBefore + 12, db.GetAccounts()[4].Balance);
        }


        [TestMethod]
        public void TestLock()
        // List all acounts  and their balance for given user.
        {
            var db = new DatabaseMockup();
            var controller = new HomeController(db);
            var model = new MainMenuModel();
            model.UserID = 1;
            model.UserName = "Orvar Slusk";
            model.SelectedAccount = 1;

            var result = controller.Lock(model) as RedirectToRouteResult;

            Assert.AreEqual("", result.RouteName);
            var account = db.GetAccounts().Where(a => a.ID == 1).Single();
            Assert.IsTrue(account.Locked);
        }
    }

}
