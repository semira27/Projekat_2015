namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zajam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sektori", "IsStudentskiZajam", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sektori", "IsStudentskiZajam");
        }
    }
}
