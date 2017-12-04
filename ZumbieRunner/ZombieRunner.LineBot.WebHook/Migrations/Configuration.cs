namespace ZombieRunner.Webhook.Migrations
{
    using System.Data.Entity.Migrations;
    using ZombieRunner.LineBot.WebHook.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ZombieRunnerDbContex>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ZombieRunnerDbContex context)
        {
            //if (!context.User.Any())
            //{
            //    context.User.AddOrUpdate(new User[] {
            //        new User{
            //            LineUserId = "U78ff37f122930697fd348c9a77a742be",
            //            Name = "test!!"
            //        }
            //    });
            //    context.SaveChanges();
            //}
        }
    }
}
