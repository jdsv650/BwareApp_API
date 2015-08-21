namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Bridge_IncludeUserVerifications_ExcludeStateID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bridges", "User1Verified", c => c.String());
            AddColumn("dbo.Bridges", "User2Verified", c => c.String());
            AddColumn("dbo.Bridges", "User3Verified", c => c.String());
            DropColumn("dbo.Bridges", "StateId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bridges", "StateId", c => c.Int());
            DropColumn("dbo.Bridges", "User3Verified");
            DropColumn("dbo.Bridges", "User2Verified");
            DropColumn("dbo.Bridges", "User1Verified");
        }
    }
}
