namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postavke : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postavke", "NaslovnaTekst", c => c.String());
            AddColumn("dbo.Postavke", "DokumentiTekst", c => c.String());
            AddColumn("dbo.Postavke", "NostrifikacijaTekst", c => c.String());
            AddColumn("dbo.Postavke", "PitanjaOdgovoriTekst", c => c.String());
            AddColumn("dbo.Postavke", "LinkoviTekst", c => c.String());
            AddColumn("dbo.Postavke", "ZajmoviTekst", c => c.String());
            AddColumn("dbo.Postavke", "RegistarTekst", c => c.String());
            AddColumn("dbo.Postavke", "NaslovnaIsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postavke", "DokumentiIsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postavke", "NostrifikacijaIsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postavke", "PitanjaOdgovoriIsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postavke", "LinkoviIsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postavke", "ZajmoviIsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Postavke", "RegistarIsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Postavke", "RegistarIsActive");
            DropColumn("dbo.Postavke", "ZajmoviIsActive");
            DropColumn("dbo.Postavke", "LinkoviIsActive");
            DropColumn("dbo.Postavke", "PitanjaOdgovoriIsActive");
            DropColumn("dbo.Postavke", "NostrifikacijaIsActive");
            DropColumn("dbo.Postavke", "DokumentiIsActive");
            DropColumn("dbo.Postavke", "NaslovnaIsActive");
            DropColumn("dbo.Postavke", "RegistarTekst");
            DropColumn("dbo.Postavke", "ZajmoviTekst");
            DropColumn("dbo.Postavke", "LinkoviTekst");
            DropColumn("dbo.Postavke", "PitanjaOdgovoriTekst");
            DropColumn("dbo.Postavke", "NostrifikacijaTekst");
            DropColumn("dbo.Postavke", "DokumentiTekst");
            DropColumn("dbo.Postavke", "NaslovnaTekst");
        }
    }
}
