namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Obavijesti", "Nova", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sektori", "Redoslijed", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sektori", "Redoslijed");
            DropColumn("dbo.Obavijesti", "Nova");
        }
    }
}
