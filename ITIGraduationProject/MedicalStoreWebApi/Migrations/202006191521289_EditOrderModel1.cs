namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOrderModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderAddress", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "ContactPhone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ContactPhone");
            DropColumn("dbo.Orders", "OrderAddress");
        }
    }
}
