namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveZaVideoMat : DbMigration
    {
        public override void Up()
        {     
            AlterColumn("dbo.VideoMaterijali", "IsActive", c => c.Boolean(nullable: false));
            
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VideoMaterijali", "IsActive", c => c.Boolean());
        }
    }
}
