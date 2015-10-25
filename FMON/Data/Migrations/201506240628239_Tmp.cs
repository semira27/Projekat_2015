namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tmp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnketaOdgovor",
                c => new
                    {
                        OdgovorID = c.Int(nullable: false, identity: true),
                        AnketaID = c.Int(),
                        Naziv = c.String(maxLength: 250),
                        BrojGlasova = c.Int(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.OdgovorID)
                .ForeignKey("dbo.Anketa", t => t.AnketaID)
                .Index(t => t.AnketaID);
            
            CreateTable(
                "dbo.Anketa",
                c => new
                    {
                        AnketaID = c.Int(nullable: false, identity: true),
                        Pitanje = c.String(),
                        DatumObjave = c.DateTime(storeType: "date"),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.AnketaID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.Korisnici",
                c => new
                    {
                        KorisnikID = c.Int(nullable: false, identity: true),
                        Ime = c.String(),
                        Prezime = c.String(),
                        KorisnickoIme = c.String(),
                        LozinkaHash = c.String(),
                        LozinkaSalt = c.String(),
                        Email = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.KorisnikID);
            
            CreateTable(
                "dbo.Dokumenti",
                c => new
                    {
                        DokumentID = c.Int(nullable: false, identity: true),
                        DokumentKategorijaID = c.Int(),
                        DatumObjave = c.DateTime(storeType: "date"),
                        Lokacija = c.String(),
                        TipDokumenta = c.String(),
                        Naziv = c.String(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                        Velicina = c.Long(),
                    })
                .PrimaryKey(t => t.DokumentID)
                .ForeignKey("dbo.DokumentiKategorije", t => t.DokumentKategorijaID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.DokumentKategorijaID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.DokumentiKategorije",
                c => new
                    {
                        KategorijaDokumentaID = c.Int(nullable: false),
                        Naziv = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.KategorijaDokumentaID);
            
            CreateTable(
                "dbo.InstitucijaLinkovi",
                c => new
                    {
                        LinkID = c.Int(nullable: false, identity: true),
                        InstitucijaID = c.Int(),
                        Link = c.String(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.LinkID)
                .ForeignKey("dbo.Institucije", t => t.InstitucijaID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.InstitucijaID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.Institucije",
                c => new
                    {
                        InstitucijaID = c.Int(nullable: false, identity: true),
                        GradID = c.Int(),
                        NazivInstitucije = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.InstitucijaID)
                .ForeignKey("dbo.Gradovi", t => t.GradID)
                .Index(t => t.GradID);
            
            CreateTable(
                "dbo.Gradovi",
                c => new
                    {
                        GradID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.GradID);
            
            CreateTable(
                "dbo.KalendarObavijesti",
                c => new
                    {
                        KalendarID = c.Int(nullable: false, identity: true),
                        Datum = c.DateTime(storeType: "date"),
                        Obavijest = c.String(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.KalendarID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.Konkursi",
                c => new
                    {
                        KonkursID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                        Obavijest = c.String(),
                        Datum = c.DateTime(storeType: "date"),
                        IsMedjunarodni = c.Boolean(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.KonkursID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.KonkursiDokumenti",
                c => new
                    {
                        KonkursiDokumentiID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                        TipDokumenta = c.String(),
                        Putanja = c.String(),
                        Velicina = c.Long(),
                        IsActive = c.Boolean(),
                        KonkursID = c.Int(),
                    })
                .PrimaryKey(t => t.KonkursiDokumentiID)
                .ForeignKey("dbo.Konkursi", t => t.KonkursID)
                .Index(t => t.KonkursID);
            
            CreateTable(
                "dbo.Najave",
                c => new
                    {
                        NajavaID = c.Int(nullable: false, identity: true),
                        Datum = c.DateTime(),
                        Sazetak = c.String(),
                        Vijest = c.String(),
                        IsActive = c.String(maxLength: 10, fixedLength: true),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.NajavaID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.Obavijesti",
                c => new
                    {
                        ObavijestID = c.Int(nullable: false, identity: true),
                        NaslovObavijesti = c.String(),
                        Datum = c.String(),
                        Sazetak = c.String(),
                        Obavijest = c.String(),
                        IsIstaknuta = c.Boolean(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                        SlikaObavijesti = c.String(),
                    })
                .PrimaryKey(t => t.ObavijestID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.ObavijestiDokumenti",
                c => new
                    {
                        ObavjestiDokumentID = c.Int(nullable: false, identity: true),
                        ObavjestID = c.Int(),
                        NazivDokumenta = c.String(),
                        TipDokumenta = c.String(),
                        Velicina = c.Long(),
                        Lokacija = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.ObavjestiDokumentID)
                .ForeignKey("dbo.Obavijesti", t => t.ObavjestID)
                .Index(t => t.ObavjestID);
            
            CreateTable(
                "dbo.ObavijestiSektori",
                c => new
                    {
                        ObavijestSektorID = c.Int(nullable: false, identity: true),
                        SektorID = c.Int(),
                        ObavijestID = c.Int(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.ObavijestSektorID)
                .ForeignKey("dbo.Obavijesti", t => t.ObavijestID)
                .ForeignKey("dbo.Sektori", t => t.SektorID)
                .Index(t => t.SektorID)
                .Index(t => t.ObavijestID);
            
            CreateTable(
                "dbo.Sektori",
                c => new
                    {
                        SektorID = c.Int(nullable: false, identity: true),
                        Slika = c.String(),
                        IkonaSektora = c.String(),
                        OSektoru = c.String(),
                        NazivSektora = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.SektorID);
            
            CreateTable(
                "dbo.ObavijestiSlike",
                c => new
                    {
                        SlikaID = c.Int(nullable: false, identity: true),
                        NazivSlike = c.String(),
                        Putanja = c.String(),
                        Velicina = c.Long(),
                        Tip = c.String(maxLength: 50),
                        ObavijestID = c.Int(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.SlikaID)
                .ForeignKey("dbo.Obavijesti", t => t.ObavijestID)
                .Index(t => t.ObavijestID);
            
            CreateTable(
                "dbo.RegistarNostrifikacija",
                c => new
                    {
                        RegistarNostrifikacijaID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                        Lokacija = c.String(),
                        Velicina = c.Long(),
                        Tip = c.String(maxLength: 50),
                        RegistarNostrifikacija = c.Boolean(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.RegistarNostrifikacijaID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.VideoMaterijali",
                c => new
                    {
                        VideoMaterijaliID = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                        Link = c.String(),
                        IsActive = c.Boolean(),
                        KorisnikID = c.Int(),
                    })
                .PrimaryKey(t => t.VideoMaterijaliID)
                .ForeignKey("dbo.Korisnici", t => t.KorisnikID)
                .Index(t => t.KorisnikID);
            
            CreateTable(
                "dbo.PitanjaOdgovori",
                c => new
                    {
                        PitanjaOdgovoriID = c.Int(nullable: false, identity: true),
                        Pitanje = c.String(),
                        Odgovor = c.String(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.PitanjaOdgovoriID);
            
            CreateTable(
                "dbo.PostaviPitanje",
                c => new
                    {
                        PitanjeID = c.Int(nullable: false, identity: true),
                        Pitanje = c.String(),
                        Datum = c.DateTime(),
                        PitanjePregledano = c.Boolean(),
                        IsActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.PitanjeID);
            
            CreateTable(
                "dbo.Postavke",
                c => new
                    {
                        PostavkeID = c.Int(nullable: false, identity: true),
                        IsActivePostaviPitanje = c.Boolean(),
                        ReferentniMail1 = c.String(),
                        ReferentniMail2 = c.String(),
                        ReferentniMail3 = c.String(),
                    })
                .PrimaryKey(t => t.PostavkeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoMaterijali", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.RegistarNostrifikacija", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.ObavijestiSlike", "ObavijestID", "dbo.Obavijesti");
            DropForeignKey("dbo.ObavijestiSektori", "SektorID", "dbo.Sektori");
            DropForeignKey("dbo.ObavijestiSektori", "ObavijestID", "dbo.Obavijesti");
            DropForeignKey("dbo.ObavijestiDokumenti", "ObavjestID", "dbo.Obavijesti");
            DropForeignKey("dbo.Obavijesti", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.Najave", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.Konkursi", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.KonkursiDokumenti", "KonkursID", "dbo.Konkursi");
            DropForeignKey("dbo.KalendarObavijesti", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.InstitucijaLinkovi", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.InstitucijaLinkovi", "InstitucijaID", "dbo.Institucije");
            DropForeignKey("dbo.Institucije", "GradID", "dbo.Gradovi");
            DropForeignKey("dbo.Dokumenti", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.Dokumenti", "DokumentKategorijaID", "dbo.DokumentiKategorije");
            DropForeignKey("dbo.Anketa", "KorisnikID", "dbo.Korisnici");
            DropForeignKey("dbo.AnketaOdgovor", "AnketaID", "dbo.Anketa");
            DropIndex("dbo.VideoMaterijali", new[] { "KorisnikID" });
            DropIndex("dbo.RegistarNostrifikacija", new[] { "KorisnikID" });
            DropIndex("dbo.ObavijestiSlike", new[] { "ObavijestID" });
            DropIndex("dbo.ObavijestiSektori", new[] { "ObavijestID" });
            DropIndex("dbo.ObavijestiSektori", new[] { "SektorID" });
            DropIndex("dbo.ObavijestiDokumenti", new[] { "ObavjestID" });
            DropIndex("dbo.Obavijesti", new[] { "KorisnikID" });
            DropIndex("dbo.Najave", new[] { "KorisnikID" });
            DropIndex("dbo.KonkursiDokumenti", new[] { "KonkursID" });
            DropIndex("dbo.Konkursi", new[] { "KorisnikID" });
            DropIndex("dbo.KalendarObavijesti", new[] { "KorisnikID" });
            DropIndex("dbo.Institucije", new[] { "GradID" });
            DropIndex("dbo.InstitucijaLinkovi", new[] { "KorisnikID" });
            DropIndex("dbo.InstitucijaLinkovi", new[] { "InstitucijaID" });
            DropIndex("dbo.Dokumenti", new[] { "KorisnikID" });
            DropIndex("dbo.Dokumenti", new[] { "DokumentKategorijaID" });
            DropIndex("dbo.Anketa", new[] { "KorisnikID" });
            DropIndex("dbo.AnketaOdgovor", new[] { "AnketaID" });
            DropTable("dbo.Postavke");
            DropTable("dbo.PostaviPitanje");
            DropTable("dbo.PitanjaOdgovori");
            DropTable("dbo.VideoMaterijali");
            DropTable("dbo.RegistarNostrifikacija");
            DropTable("dbo.ObavijestiSlike");
            DropTable("dbo.Sektori");
            DropTable("dbo.ObavijestiSektori");
            DropTable("dbo.ObavijestiDokumenti");
            DropTable("dbo.Obavijesti");
            DropTable("dbo.Najave");
            DropTable("dbo.KonkursiDokumenti");
            DropTable("dbo.Konkursi");
            DropTable("dbo.KalendarObavijesti");
            DropTable("dbo.Gradovi");
            DropTable("dbo.Institucije");
            DropTable("dbo.InstitucijaLinkovi");
            DropTable("dbo.DokumentiKategorije");
            DropTable("dbo.Dokumenti");
            DropTable("dbo.Korisnici");
            DropTable("dbo.Anketa");
            DropTable("dbo.AnketaOdgovor");
        }
    }
}
