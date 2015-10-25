namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class linkovi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Institucije", "IsGeneralniLink", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Institucije", "IsGeneralniLink");
        }
    }
}
