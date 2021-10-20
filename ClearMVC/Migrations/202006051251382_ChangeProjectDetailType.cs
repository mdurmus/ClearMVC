namespace ClearMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProjectDetailType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectDetailsTypes", "FirmaId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectDetailsTypes", "FirmaId");
            AddForeignKey("dbo.ProjectDetailsTypes", "FirmaId", "dbo.Firmas", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectDetailsTypes", "FirmaId", "dbo.Firmas");
            DropIndex("dbo.ProjectDetailsTypes", new[] { "FirmaId" });
            DropColumn("dbo.ProjectDetailsTypes", "FirmaId");
        }
    }
}
