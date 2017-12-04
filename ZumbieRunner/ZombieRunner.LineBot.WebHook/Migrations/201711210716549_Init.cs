namespace ZombieRunner.LineBot.WebHook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SignupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signups", t => t.SignupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SignupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Signups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineUserId = c.String(maxLength: 50),
                        Name = c.String(maxLength: 100),
                        Level = c.Int(),
                        Experience = c.Int(),
                        GameCounts = c.Int(),
                        IsEnabled = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameStoryLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SignupId = c.Int(nullable: false),
                        DayNumber = c.Int(nullable: false),
                        StoryFrameId = c.Int(nullable: false),
                        Weather = c.Int(nullable: false),
                        Detail = c.String(),
                        IsEnabled = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Signups", t => t.SignupId, cascadeDelete: true)
                .ForeignKey("dbo.StoryFrames", t => t.StoryFrameId, cascadeDelete: true)
                .Index(t => t.SignupId)
                .Index(t => t.StoryFrameId);
            
            CreateTable(
                "dbo.StoryFrames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Weather = c.Int(),
                        Content = c.String(),
                        IsEnabled = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameStoryLogs", "StoryFrameId", "dbo.StoryFrames");
            DropForeignKey("dbo.GameStoryLogs", "SignupId", "dbo.Signups");
            DropForeignKey("dbo.GameMembers", "UserId", "dbo.Users");
            DropForeignKey("dbo.GameMembers", "SignupId", "dbo.Signups");
            DropIndex("dbo.GameStoryLogs", new[] { "StoryFrameId" });
            DropIndex("dbo.GameStoryLogs", new[] { "SignupId" });
            DropIndex("dbo.GameMembers", new[] { "UserId" });
            DropIndex("dbo.GameMembers", new[] { "SignupId" });
            DropTable("dbo.StoryFrames");
            DropTable("dbo.GameStoryLogs");
            DropTable("dbo.Users");
            DropTable("dbo.Signups");
            DropTable("dbo.GameMembers");
        }
    }
}
