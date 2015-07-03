namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stateId_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bridges", "StateId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bridges", "StateId", c => c.Int(nullable: false));
        }
    }
}
