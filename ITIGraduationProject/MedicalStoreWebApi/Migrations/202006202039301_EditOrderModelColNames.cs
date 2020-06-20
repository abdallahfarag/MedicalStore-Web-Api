namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOrderModelColNames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "DateAdded", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "DateAdded", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
