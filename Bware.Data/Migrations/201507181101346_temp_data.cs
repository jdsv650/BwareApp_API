namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp_data : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Temps",
                c => new
                    {
                        TempId = c.Int(nullable: false, identity: true),
                        TempString = c.String(),
                    })
                .PrimaryKey(t => t.TempId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Temps");
        }
    }
}
