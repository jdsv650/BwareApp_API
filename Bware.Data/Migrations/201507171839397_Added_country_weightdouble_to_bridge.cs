namespace Bware.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_country_weightdouble_to_bridge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bridges", "Country", c => c.String());
            AddColumn("dbo.Bridges", "WeightDouble", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bridges", "WeightDouble");
            DropColumn("dbo.Bridges", "Country");
        }
    }
}
