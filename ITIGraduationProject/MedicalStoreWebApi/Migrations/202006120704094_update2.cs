namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "TotalPrice", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.Orders", "DateAdded", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DateAdded", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "TotalPrice", c => c.Int(nullable: false));
        }
    }
}
