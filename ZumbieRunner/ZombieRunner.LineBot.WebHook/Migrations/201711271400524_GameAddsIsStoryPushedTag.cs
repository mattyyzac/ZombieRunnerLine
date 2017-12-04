namespace ZombieRunner.Webhook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameAddsIsStoryPushedTag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Signups", "IsStoryPushed", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Signups", "IsStoryPushed");
        }
    }
}
