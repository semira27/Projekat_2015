namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class slike : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ObavijestiSlike", "NazivSlike");
            DropColumn("dbo.ObavijestiSlike", "Velicina");
            DropColumn("dbo.ObavijestiSlike", "Tip");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ObavijestiSlike", "Tip", c => c.String(maxLength: 50));
            AddColumn("dbo.ObavijestiSlike", "Velicina", c => c.Long(nullable: false));
            AddColumn("dbo.ObavijestiSlike", "NazivSlike", c => c.String());
        }
    }
}
