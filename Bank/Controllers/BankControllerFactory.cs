using Bank.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bank.Controllers
{

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
            if (!controllers.ContainsKey(controllerName)) return null;

            return controllers[controllerName](requestContext);
        }


        public static IControllerFactory GetControllerFactory()
        {
            string typeName = ConfigurationManager.AppSettings["repository"];
            var type = Type.GetType(typeName);
            var repository = Activator.CreateInstance(type);
            return new BankControllerFactory(repository as IBankDbContext);
        }
    }
}