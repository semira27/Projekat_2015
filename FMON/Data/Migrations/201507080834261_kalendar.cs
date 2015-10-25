namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KalendarObavijesti", "Naslov", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KalendarObavijesti", "Naslov");
        }
    }
}
