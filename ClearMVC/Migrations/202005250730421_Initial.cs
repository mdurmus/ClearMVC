namespace ClearMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivitiyId = c.Int(nullable: false, identity: true),
                        FirmaId = c.Int(),
                        Subject = c.String(maxLength: 1000),
                        ActivityText = c.String(),
                        ForCustomer = c.Boolean(nullable: false),
                        ActivityDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(),
                        CreatedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ActivitiyId)
                .ForeignKey("dbo.Firmas", t => t.FirmaId)
                .Index(t => t.FirmaId);
            
            CreateTable(
                "dbo.Firmas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        VATCode = c.String(),
                        Email = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                        City = c.String(),
                        Area = c.String(),
                        Phone = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomersId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Phone = c.String(),
                        PostalCode = c.String(),
                        Strasse = c.String(),
                        City = c.String(),
                        Number = c.String(),
                        Area = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        FirmaId = c.Int(),
                    })
                .PrimaryKey(t => t.CustomersId)
                .ForeignKey("dbo.Firmas", t => t.FirmaId)
                .Index(t => t.FirmaId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectsId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 1000),
                        IsActive = c.Boolean(),
                        CreateDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Location = c.String(),
                        CustomerId = c.Int(),
                        FirmaId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectsId)
                .ForeignKey("dbo.Firmas", t => t.FirmaId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId)
                .Index(t => t.FirmaId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactsId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(),
                        Name = c.String(maxLength: 500),
                        SurName = c.String(maxLength: 500),
                        GSM = c.String(maxLength: 20, fixedLength: true),
                        Email = c.String(maxLength: 500),
                        IsActive = c.Boolean(),
                        CreateDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ContactsId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectDetails",
                c => new
                    {
                        ProjectDetailsId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(),
                        PersonId = c.Int(),
                        ProjectDetailTypeId = c.Int(),
                        StartDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        FinishDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Square = c.Int(),
                        Duration = c.Double(),
                        IsActive = c.Boolean(),
                        IsCompleted = c.Boolean(),
                        IsRefuse = c.Boolean(nullable: false),
                        ForRefuse = c.Boolean(nullable: false),
                        RepeatMonth = c.Int(),
                        CompleteDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(maxLength: 100),
                        IsCloseRefuseByCustomer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectDetailsId)
                .ForeignKey("dbo.ProjectDetailsTypes", t => t.ProjectDetailTypeId)
                .ForeignKey("dbo.Users", t => t.PersonId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.PersonId)
                .Index(t => t.ProjectDetailTypeId);
            
            CreateTable(
                "dbo.ProjectDetailsTypes",
                c => new
                    {
                        ProjectDetailsTypesId = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 500),
                        IsActive = c.Boolean(),
                        CreatedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ProjectDetailsTypesId);
            
            CreateTable(
                "dbo.Refuses",
                c => new
                    {
                        RefuseId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        RefuseText = c.String(),
                        FileName = c.String(),
                        CreatedBy = c.String(),
                        LeaveDate = c.DateTime(nullable: false),
                        ProjectDetailsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RefuseId)
                .ForeignKey("dbo.ProjectDetails", t => t.ProjectDetailsId, cascadeDelete: true)
                .Index(t => t.ProjectDetailsId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UsersId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                        LastName = c.String(maxLength: 500),
                        Email = c.String(maxLength: 500),
                        Password = c.String(maxLength: 500),
                        IsActive = c.Boolean(),
                        CreateDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        FirmaId = c.Int(),
                        UserTypesId = c.Int(),
                    })
                .PrimaryKey(t => t.UsersId)
                .ForeignKey("dbo.Firmas", t => t.FirmaId)
                .ForeignKey("dbo.UserTypes", t => t.UserTypesId)
                .Index(t => t.FirmaId)
                .Index(t => t.UserTypesId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessagesId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        MessageText = c.String(),
                        IsRead = c.Boolean(),
                        IsActive = c.Boolean(),
                        LeaveDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Sender = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.MessagesId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserDetailsId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        Street = c.String(),
                        Number = c.String(),
                        City = c.String(),
                        Area = c.String(),
                        GSM = c.String(maxLength: 128, fixedLength: true),
                        IsActive = c.Boolean(),
                        CreateDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UserDetailsId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        UserTypesId = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 500),
                        IsActive = c.Boolean(),
                        CreateDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UserTypesId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        FirmaId = c.Int(),
                        Subject = c.String(maxLength: 1000),
                        ForCustomer = c.Boolean(nullable: false),
                        Text = c.String(),
                        IsActive = c.Boolean(),
                        CreateDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("dbo.Firmas", t => t.FirmaId)
                .Index(t => t.FirmaId);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "FirmaId", "dbo.Firmas");
            DropForeignKey("dbo.Projects", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.ProjectDetails", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Users", "UserTypesId", "dbo.UserTypes");
            DropForeignKey("dbo.UserDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectDetails", "PersonId", "dbo.Users");
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "FirmaId", "dbo.Firmas");
            DropForeignKey("dbo.Refuses", "ProjectDetailsId", "dbo.ProjectDetails");
            DropForeignKey("dbo.ProjectDetails", "ProjectDetailTypeId", "dbo.ProjectDetailsTypes");
            DropForeignKey("dbo.Projects", "FirmaId", "dbo.Firmas");
            DropForeignKey("dbo.Contacts", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Customers", "FirmaId", "dbo.Firmas");
            DropForeignKey("dbo.Activities", "FirmaId", "dbo.Firmas");
            DropIndex("dbo.News", new[] { "FirmaId" });
            DropIndex("dbo.UserDetails", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "UserTypesId" });
            DropIndex("dbo.Users", new[] { "FirmaId" });
            DropIndex("dbo.Refuses", new[] { "ProjectDetailsId" });
            DropIndex("dbo.ProjectDetails", new[] { "ProjectDetailTypeId" });
            DropIndex("dbo.ProjectDetails", new[] { "PersonId" });
            DropIndex("dbo.ProjectDetails", new[] { "ProjectId" });
            DropIndex("dbo.Contacts", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "FirmaId" });
            DropIndex("dbo.Projects", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "FirmaId" });
            DropIndex("dbo.Activities", new[] { "FirmaId" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.News");
            DropTable("dbo.UserTypes");
            DropTable("dbo.UserDetails");
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.Refuses");
            DropTable("dbo.ProjectDetailsTypes");
            DropTable("dbo.ProjectDetails");
            DropTable("dbo.Contacts");
            DropTable("dbo.Projects");
            DropTable("dbo.Customers");
            DropTable("dbo.Firmas");
            DropTable("dbo.Activities");
        }
    }
}
