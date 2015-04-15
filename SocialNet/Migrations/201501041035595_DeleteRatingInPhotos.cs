namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRatingInPhotos : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Photos", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "Rating", c => c.Int(nullable: false));
        }
    }
}
