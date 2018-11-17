namespace ClassLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        ShortDescription = c.String(nullable: false, maxLength: 500),
                        LongDescription = c.String(nullable: false, maxLength: 4000),
                        Date = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        Picture1 = c.Binary(),
                        Picture2 = c.Binary(),
                        Picture3 = c.Binary(),
                        state = c.Int(nullable: false),
                        OwnerID_ID = c.Long(),
                        UserID_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.OwnerID_ID)
                .ForeignKey("dbo.Users", t => t.UserID_ID)
                .Index(t => t.OwnerID_ID)
                .Index(t => t.UserID_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Birthdate = c.DateTime(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "UserID_ID", "dbo.Users");
            DropForeignKey("dbo.Products", "OwnerID_ID", "dbo.Users");
            DropIndex("dbo.Products", new[] { "UserID_ID" });
            DropIndex("dbo.Products", new[] { "OwnerID_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.Products");
        }
    }
}
