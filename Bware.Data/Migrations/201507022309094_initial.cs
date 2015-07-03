namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bridges",
                c => new
                    {
                        BridgeId = c.Int(nullable: false, identity: true),
                        BIN = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        BridgeLocation = c.Geography(),
                        FeatureCarried = c.String(),
                        FeatureCrossed = c.String(),
                        State = c.String(),
                        County = c.String(),
                        Township = c.String(),
                        Zip = c.String(),
                        Height = c.Double(),
                        WeightStraight = c.Double(),
                        WeightCombination = c.Double(),
                        isRposted = c.Boolean(nullable: false),
                        OtherPosting = c.String(),
                        StateId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        NumberOfVotes = c.Int(nullable: false),
                        isLocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BridgeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bridges");
        }
    }
}
