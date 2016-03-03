using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using Bank.Database;

namespace Bank.Models
{
    public class HomeViewModel
    {
        public string UserName { get; set; }

        public SelectList Users { set; get; }

        public  void SetUserOptions(IBankDbContext db)
        {
            var dict = db.GetUsers().ToDictionary(g => g.ID, g => g.Name);
            Users = new SelectList(dict, "key", "value");
        }
    }
}