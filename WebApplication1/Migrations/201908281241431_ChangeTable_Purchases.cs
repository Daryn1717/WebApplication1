namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTable_Purchases : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchases", "Term", c => c.Int(nullable: false));
            AddColumn("dbo.Purchases", "DateOfPurchase", c => c.DateTime(nullable: false));
            AddColumn("dbo.Purchases", "DateOfReturn", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchases", "DateOfReturn");
            DropColumn("dbo.Purchases", "DateOfPurchase");
            DropColumn("dbo.Purchases", "Term");
        }
    }
}
