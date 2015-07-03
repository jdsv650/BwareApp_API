namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLocationDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bridges", "LocationDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bridges", "LocationDescription");
        }
    }
}
