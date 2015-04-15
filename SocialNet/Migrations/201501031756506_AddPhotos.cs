namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                        User_UserId1 = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.Users", t => t.User_UserId1)
                .Index(t => t.User_UserId)
                .Index(t => t.User_UserId1);
            
            AddColumn("dbo.Users", "PhotoId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Avatar_PhotoId", c => c.Int());
            AddForeignKey("dbo.Users", "Avatar_PhotoId", "dbo.Photos", "PhotoId");
            CreateIndex("dbo.Users", "Avatar_PhotoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Photos", new[] { "User_UserId1" });
            DropIndex("dbo.Photos", new[] { "User_UserId" });
            DropIndex("dbo.Users", new[] { "Avatar_PhotoId" });
            DropForeignKey("dbo.Photos", "User_UserId1", "dbo.Users");
            DropForeignKey("dbo.Photos", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Avatar_PhotoId", "dbo.Photos");
            DropColumn("dbo.Users", "Avatar_PhotoId");
            DropColumn("dbo.Users", "PhotoId");
            DropTable("dbo.Photos");
        }
    }
}
