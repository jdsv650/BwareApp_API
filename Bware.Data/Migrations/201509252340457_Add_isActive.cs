namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_isActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bridges", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bridges", "isActive");
        }
    }
}
