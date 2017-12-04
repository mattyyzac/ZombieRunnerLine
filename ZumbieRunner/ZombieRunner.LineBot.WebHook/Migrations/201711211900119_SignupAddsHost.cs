namespace ZombieRunner.LineBot.WebHook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignupAddsHost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Signups", "HostUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Signups", "HostUserId");
        }
    }
}
