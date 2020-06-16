namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestUpdateproduct : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", "IX_UniqueKeyString");
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Image", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Image", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Products", "Name", unique: true, name: "IX_UniqueKeyString");
        }
    }
}
