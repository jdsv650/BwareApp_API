namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_WeightStraight_TriAxle_to_bridge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bridges", "WeightStraight_TriAxle", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bridges", "WeightStraight_TriAxle");
        }
    }
}
