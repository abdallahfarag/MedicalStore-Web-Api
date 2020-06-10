namespace MedicalStoreWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserData : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Fname");
            DropColumn("dbo.AspNetUsers", "Lname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Lname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Fname", c => c.String());
        }
    }
}
