namespace ClearMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRepeatMonthFromProjectDetails : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProjectDetails", "RepeatMonth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectDetails", "RepeatMonth", c => c.Int());
        }
    }
}
