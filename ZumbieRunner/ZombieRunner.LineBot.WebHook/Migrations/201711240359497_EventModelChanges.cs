namespace ZombieRunner.LineBot.WebHook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Weather", c => c.Int());
            AlterColumn("dbo.Dinners", "Seed", c => c.Long(nullable: false));
            AlterColumn("dbo.Events", "Seed", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Seed", c => c.Int(nullable: false));
            AlterColumn("dbo.Dinners", "Seed", c => c.Int(nullable: false));
            DropColumn("dbo.Events", "Weather");
        }
    }
}
