namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADD_Message : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        MessageLocation = c.Geography(),
                        MessageText = c.String(),
                        UserCreated = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
