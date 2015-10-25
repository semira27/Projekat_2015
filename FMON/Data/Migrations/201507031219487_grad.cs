namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Institucije", "GradID", "dbo.Gradovi");
            DropForeignKey("dbo.InstitucijaLinkovi", "InstitucijaID", "dbo.Institucije");
            DropIndex("dbo.InstitucijaLinkovi", new[] { "InstitucijaID" });
            DropIndex("dbo.Institucije", new[] { "GradID" });
            AddColumn("dbo.InstitucijaLinkovi", "GradID", c => c.Int(nullable: false));
            AddColumn("dbo.InstitucijaLinkovi", "NazivLinka", c => c.String());
            AddColumn("dbo.Gradovi", "IsGeneralno", c => c.Boolean(nullable: false));
            CreateIndex("dbo.InstitucijaLinkovi", "GradID");
            AddForeignKey("dbo.InstitucijaLinkovi", "GradID", "dbo.Gradovi", "GradID", cascadeDelete: true);
            DropColumn("dbo.InstitucijaLinkovi", "InstitucijaID");
            DropTable("dbo.Institucije");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Institucije",
                c => new
                    {
                        InstitucijaID = c.Int(nullable: false, identity: true),
                        GradID = c.Int(nullable: false),
                        NazivInstitucije = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsGeneralniLink = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InstitucijaID);
            
            AddColumn("dbo.InstitucijaLinkovi", "InstitucijaID", c => c.Int(nullable: false));
            DropForeignKey("dbo.InstitucijaLinkovi", "GradID", "dbo.Gradovi");
            DropIndex("dbo.InstitucijaLinkovi", new[] { "GradID" });
            DropColumn("dbo.Gradovi", "IsGeneralno");
            DropColumn("dbo.InstitucijaLinkovi", "NazivLinka");
            DropColumn("dbo.InstitucijaLinkovi", "GradID");
            CreateIndex("dbo.Institucije", "GradID");
            CreateIndex("dbo.InstitucijaLinkovi", "InstitucijaID");
            AddForeignKey("dbo.InstitucijaLinkovi", "InstitucijaID", "dbo.Institucije", "InstitucijaID", cascadeDelete: true);
            AddForeignKey("dbo.Institucije", "GradID", "dbo.Gradovi", "GradID", cascadeDelete: true);
        }
    }
}
