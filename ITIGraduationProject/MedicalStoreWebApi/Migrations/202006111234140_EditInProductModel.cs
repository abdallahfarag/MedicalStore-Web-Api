namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditInProductModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "QuantityInStock", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "QuantityInStock");
        }
    }
}
