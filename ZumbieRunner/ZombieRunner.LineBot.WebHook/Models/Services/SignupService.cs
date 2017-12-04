using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class SignupService
    {
        public async Task<int> CreateAsync(string lineUserId, string desc)
        {
            if (string.IsNullOrEmpty(lineUserId)) return -1;

            using (var db = new ZombieRunnerDbContex())
            {
                var now = DateTime.UtcNow;
                var b = await db.Signup.AnyAsync(o => o.HostUserId == lineUserId && o.EndTime >= now && o.IsEnabled == true && o.IsDeleted != true);
                await Cores.Logger.WriteInfoLog($"CreateAsync, b = {b}");
                if (b)
                {
                    //  已有報名，do nothing
                    return -1;
                }
                else
                {
                    var signup = new Signup
                    {
                        HostUserId = lineUserId,
                        Description = desc,
                        StartTime = now,
                        EndTime = now.AddMinutes(30), // 報名截止

                        StoryBeginTime = now.AddMinutes(31), // 開團，第 31 分鐘動啟故事訊息推送
                        StoryEndedTime = now.AddMinutes(65), // 遊戲循環結束

                        IsEnabled = true,
                        CreatedOn = now,
                        ModifiedOn = now
                    };
                    db.Entry(signup).State = EntityState.Added;
                    await db.SaveChangesAsync();

                    return signup.Id;
                }
            }
        }

        public async Task<Signup> FindLatestForSignupAsync()
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var now = DateTime.UtcNow;
                var game = await db.Signup.FirstOrDefaultAsync(o => o.StartTime <= now && o.EndTime >= now && o.IsEnabled == true && o.IsDeleted != true && o.IsOnGoing != true && o.IsStoryPushed != true);
                return game;
            }
        }

        public async Task SetToBeginAsync(Signup game)
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var gameToStart = await db.Signup.FirstOrDefaultAsync(o => o.Id == game.Id);
                if (gameToStart != null)
                {
                    gameToStart.IsOnGoing = true;
                    var now = DateTime.UtcNow;
                    gameToStart.StoryBeginTime = now;
                    gameToStart.StoryEndedTime = now.AddMinutes(60);
                    db.Entry(gameToStart).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task ResetToEndAsync(Signup game)
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var gameToEnd = await db.Signup.FirstOrDefaultAsync(o => o.Id == game.Id);
                if (gameToEnd != null)
                {
                    gameToEnd.IsStoryPushed = true;
                    //gameToEnd.StoryEndedTime = DateTime.UtcNow;
                    db.Entry(gameToEnd).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
            }
        }

    }
}