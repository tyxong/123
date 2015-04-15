namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePicture : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Avatar_PhotoId", "dbo.Photos");
            DropForeignKey("dbo.Photos", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Photos", "User_UserId1", "dbo.Users");
            DropIndex("dbo.Users", new[] { "Avatar_PhotoId" });
            DropIndex("dbo.Photos", new[] { "User_UserId" });
            DropIndex("dbo.Photos", new[] { "User_UserId1" });
            RenameColumn(table: "dbo.Photos", name: "User_UserId", newName: "UserId");
            AddColumn("dbo.Photos", "IsAvatar", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.Photos", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            CreateIndex("dbo.Photos", "UserId");
            DropColumn("dbo.Users", "PhotoId");
            DropColumn("dbo.Users", "Avatar_PhotoId");
            DropColumn("dbo.Photos", "User_UserId1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "User_UserId1", c => c.Int());
            AddColumn("dbo.Users", "Avatar_PhotoId", c => c.Int());
            AddColumn("dbo.Users", "PhotoId", c => c.Int(nullable: false));
            DropIndex("dbo.Photos", new[] { "UserId" });
            DropForeignKey("dbo.Photos", "UserId", "dbo.Users");
            DropColumn("dbo.Photos", "IsAvatar");
            RenameColumn(table: "dbo.Photos", name: "UserId", newName: "User_UserId");
            CreateIndex("dbo.Photos", "User_UserId1");
            CreateIndex("dbo.Photos", "User_UserId");
            CreateIndex("dbo.Users", "Avatar_PhotoId");
            AddForeignKey("dbo.Photos", "User_UserId1", "dbo.Users", "UserId");
            AddForeignKey("dbo.Photos", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Users", "Avatar_PhotoId", "dbo.Photos", "PhotoId");
        }
    }
}
