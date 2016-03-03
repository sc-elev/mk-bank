using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bank.Database
{

    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public User()
        {
            Accounts = new HashSet<Account>();
        }
    }


    public class Account
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public bool Locked { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }
    }


    public class Transaction
    {
        [Key]
        public int ID { get; set; }

        public string Note { get; set; }

        public Account From { get; set; }

        public Account To { get; set; }

        public Decimal Amount { get; set; }
    }


    public interface IBankDbContext
    {
        IList<User> GetUsers();
        IList<Account> GetAccounts();
        IList<Transaction> GetTransactions();
    }


    public class BankDbContext : DbContext, IBankDbContext
    {
        public BankDbContext() : base("DefaultConnection") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public IList<User> GetUsers()
            { return Users.Local.ToList(); }

        public IList<Account> GetAccounts()
            { return Accounts.Local.ToList(); }

        public IList<Transaction> GetTransactions()
            { return Transactions.Local.ToList(); }
    }
}
