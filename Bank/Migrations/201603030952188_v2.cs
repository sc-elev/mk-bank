namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Name", c => c.String());
            AddColumn("dbo.Transactions", "Note", c => c.String());
            AddColumn("dbo.Transactions", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Amount");
            DropColumn("dbo.Transactions", "Note");
            DropColumn("dbo.Accounts", "Name");
        }
    }
}
