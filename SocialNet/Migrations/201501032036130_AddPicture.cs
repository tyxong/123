namespace SocialNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Picture", c => c.Binary());
            AddColumn("dbo.Photos", "ImageMimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "ImageMimeType");
            DropColumn("dbo.Photos", "Picture");
        }
    }
}
