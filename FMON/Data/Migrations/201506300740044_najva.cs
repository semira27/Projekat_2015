namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class najva : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Obavijesti", "Datum", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Obavijesti", "Datum", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
