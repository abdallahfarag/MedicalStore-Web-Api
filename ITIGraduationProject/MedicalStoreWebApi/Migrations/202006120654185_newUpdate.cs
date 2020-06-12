namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalPrice = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        IsDelivered = c.Boolean(nullable: false),
                        FeedBack = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropTable("dbo.Orders");
        }
    }
}
