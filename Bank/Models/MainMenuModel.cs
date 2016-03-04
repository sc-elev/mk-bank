using Bank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bank.Models
{



    public class MainMenuModel
    {
        public string UserName { get; set; }

        public int UserID { get; set; }

        public int SelectedAccount { get; set; }

        public int FromAccount { get; set; }

        public int ToAccount { get; set; }

        public int Amount { get; set; }
    }
}