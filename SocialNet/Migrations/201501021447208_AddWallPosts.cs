namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWallPosts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WallPosts",
                c => new
                    {
                        WallPostId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Rating = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WallPostId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.WallPosts", new[] { "UserId" });
            DropForeignKey("dbo.WallPosts", "UserId", "dbo.Users");
            DropTable("dbo.WallPosts");
        }
    }
}
