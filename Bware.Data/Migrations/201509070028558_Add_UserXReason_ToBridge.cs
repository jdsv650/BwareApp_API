namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserXReason_ToBridge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bridges", "User1Reason", c => c.Boolean());
            AddColumn("dbo.Bridges", "User2Reason", c => c.Boolean());
            AddColumn("dbo.Bridges", "User3Reason", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bridges", "User3Reason");
            DropColumn("dbo.Bridges", "User2Reason");
            DropColumn("dbo.Bridges", "User1Reason");
        }
    }
}
