namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLasrVisit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastVisit", c => c.DateTime(nullable: false));
         }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastVisit");
            }
    }
}
