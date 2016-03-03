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
        }
    }
}
