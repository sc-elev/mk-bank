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
    };


    // Test implementation, cannot live in tests (instantiated using reflection).
    public class DatabaseMockup : IBankDbContext
    {

        static protected IList<Account> accounts = new List<Account> {
            new Account{
                ID = 1, Name = "Lönekonto", Balance = 0m, Locked = false},
            new Account{
                ID = 2, Name = "Sparkonto", Balance = 0m, Locked = false},
            new Account{
                ID = 3, Name = "Gemensamt", Balance = 0m, Locked = false},
            new Account{
                ID = 4, Name = "Nöjen & sånt", Balance = 0m, Locked = false},
            new Account{
                ID = 5, Name = "Sparkonto", Balance = 0m, Locked = false},
            new Account{
                ID = 6, Name = "Mat", Balance = 0m, Locked = false},
            new Account{
                ID = 7, Name = "Firman", Balance = 0m, Locked = false},
            new Account{
                ID = 8, Name = "Kassa", Balance = 0m, Locked = false}
        };


        protected IList<Transaction> transactions = new List<Transaction> {
            new Transaction{
                ID = 1, Amount = 26000, From = accounts[7], To = accounts[0], Note = "Lön" },
            new Transaction{
                ID = 2, Amount = 8000, From = accounts[0], To = accounts[7], Note = "Hyra" },
            new Transaction{
                ID = 3, Amount = 2000, From = accounts[0], To = accounts[2], Note = "Bilen" },
            new Transaction{
                ID = 4, Amount = 4000, From = accounts[0], To = accounts[5], Note = "Mat" },
            new Transaction{
                ID = 5, Amount = 3000, From = accounts[6], To = accounts[0], Note = "Arvode" },
            new Transaction{
                ID = 6, Amount = 3000, From = accounts[0], To = accounts[4], Note = "Sparat" },
            new Transaction{
                ID = 7, Amount = 500, From = accounts[0], To = accounts[3], Note = "Bio" },
            new Transaction{
                ID = 8, Amount = 1200, From = accounts[0], To = accounts[3], Note = "Krogen" }
        };


        public IList<User> GetUsers()
        {
            return new List<User> {
                new User {ID = 1, Name = "Orvar Slusk",
                          Accounts = new List<Account> {
                              accounts[0], accounts[1], accounts[4],
                              accounts[5], accounts[6], accounts[7]
                          }
                },
                new User {ID = 2, Name= "Pelle Snusk",
                         Accounts = new List<Account> { accounts[2], accounts[3] }
                }
            };
        }


        public IList<Account> GetAccounts() { return accounts; }

        public IList<Transaction> GetTransactions() { return transactions; }

    }

}
