namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test133423 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dokumenti", "DokumentKategorijaID", "dbo.DokumentiKategorije");
            DropPrimaryKey("dbo.DokumentiKategorije");
            AlterColumn("dbo.DokumentiKategorije", "KategorijaDokumentaID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DokumentiKategorije", "KategorijaDokumentaID");
            AddForeignKey("dbo.Dokumenti", "DokumentKategorijaID", "dbo.DokumentiKategorije", "KategorijaDokumentaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dokumenti", "DokumentKategorijaID", "dbo.DokumentiKategorije");
            DropPrimaryKey("dbo.DokumentiKategorije");
            AlterColumn("dbo.DokumentiKategorije", "KategorijaDokumentaID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.DokumentiKategorije", "KategorijaDokumentaID");
            AddForeignKey("dbo.Dokumenti", "DokumentKategorijaID", "dbo.DokumentiKategorije", "KategorijaDokumentaID");
        }
    }
}
