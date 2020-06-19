namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "IsDelivered");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "IsDelivered", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "OrderStatus");
        }
    }
}
