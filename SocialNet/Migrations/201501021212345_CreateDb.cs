namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDb : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Dialogs", "DialogName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dialogs", "DialogName", c => c.String());
        }
    }
}
