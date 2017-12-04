namespace ZombieRunner.LineBot.WebHook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTwoNewTableAndAddSomeGameCols : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StoryFrames", newName: "Dinners");
            DropForeignKey("dbo.GameStoryLogs", "StoryFrameId", "dbo.StoryFrames");
            DropIndex("dbo.GameStoryLogs", new[] { "StoryFrameId" });
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seed = c.Int(nullable: false),
                        Content = c.String(),
                        IsAnyoneDead = c.Boolean(),
                        IsEnabled = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.GameMembers", "IsDead", c => c.Boolean());
            AddColumn("dbo.Signups", "IsOnGoing", c => c.Boolean());
            AddColumn("dbo.Signups", "StoryBeginTime", c => c.DateTime());
            AddColumn("dbo.Signups", "StoryEndedTime", c => c.DateTime());
            AddColumn("dbo.Dinners", "Seed", c => c.Int(nullable: false));
            DropColumn("dbo.GameStoryLogs", "StoryFrameId");
            DropColumn("dbo.GameStoryLogs", "Weather");
            DropColumn("dbo.Dinners", "Weather");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dinners", "Weather", c => c.Int());
            AddColumn("dbo.GameStoryLogs", "Weather", c => c.Int(nullable: false));
            AddColumn("dbo.GameStoryLogs", "StoryFrameId", c => c.Int(nullable: false));
            DropColumn("dbo.Dinners", "Seed");
            DropColumn("dbo.Signups", "StoryEndedTime");
            DropColumn("dbo.Signups", "StoryBeginTime");
            DropColumn("dbo.Signups", "IsOnGoing");
            DropColumn("dbo.GameMembers", "IsDead");
            DropTable("dbo.Events");
            CreateIndex("dbo.GameStoryLogs", "StoryFrameId");
            AddForeignKey("dbo.GameStoryLogs", "StoryFrameId", "dbo.StoryFrames", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.Dinners", newName: "StoryFrames");
        }
    }
}
