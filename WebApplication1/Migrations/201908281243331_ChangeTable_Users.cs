namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTable_Users : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Purchases", "DateOfReturn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Purchases", "DateOfReturn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Email");
        }
    }
}
