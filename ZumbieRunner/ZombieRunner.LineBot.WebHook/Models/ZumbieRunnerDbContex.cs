using System;
using System.Data.Entity;

namespace ZombieRunner.LineBot.WebHook.Models
{
    public class ZombieRunnerDbContex : DbContext
    {
        #region Constructor

        public ZombieRunnerDbContex()
            : base("name=DbConnection")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
        }
        #endregion
        #region Properties

        public DbSet<User> User { get; set; }
        public DbSet<Signup> Signup { get; set; }
        public DbSet<GameMember> GameMember { get; set; }
        public DbSet<GameStoryLog> GameStoryLog { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Dinner> Dinner { get; set; }

        #endregion
        #region Methods

        public static ZombieRunnerDbContex Create()
        {
            return new ZombieRunnerDbContex();
        }

        public override int SaveChanges()
        {
            var entities = this.ChangeTracker.Entries();
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.CurrentValues.SetValues(new
                    {
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow
                    });
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.CurrentValues.SetValues(new { ModifiedOn = DateTime.UtcNow });
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}