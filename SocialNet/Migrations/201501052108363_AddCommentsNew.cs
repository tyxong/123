namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentsNew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "User");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "User", c => c.String());
        }
    }
}
