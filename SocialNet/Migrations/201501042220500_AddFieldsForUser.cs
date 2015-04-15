namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsForUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CurrentCity", c => c.String());
            AddColumn("dbo.Users", "Mobile", c => c.String());
            AddColumn("dbo.Users", "Adress", c => c.String());
            AddColumn("dbo.Users", "RelationShip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RelationShip");
            DropColumn("dbo.Users", "Adress");
            DropColumn("dbo.Users", "Mobile");
            DropColumn("dbo.Users", "CurrentCity");
        }
    }
}
