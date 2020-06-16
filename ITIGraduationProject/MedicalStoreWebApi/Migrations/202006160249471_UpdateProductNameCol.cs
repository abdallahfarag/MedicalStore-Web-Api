namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductNameCol : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Products", "Image", c => c.String());
            CreateIndex("dbo.Products", "Name", unique: true, name: "IX_UniqueKeyString");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", "IX_UniqueKeyString");
            AlterColumn("dbo.Products", "Image", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
        }
    }
}
