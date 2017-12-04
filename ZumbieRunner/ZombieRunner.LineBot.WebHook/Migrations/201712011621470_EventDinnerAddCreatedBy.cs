namespace ZombieRunner.Webhook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventDinnerAddCreatedBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dinners", "CreatedBy", c => c.String());
            AddColumn("dbo.Events", "CreatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "CreatedBy");
            DropColumn("dbo.Dinners", "CreatedBy");
        }
    }
}
