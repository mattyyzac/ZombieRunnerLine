using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ZombieRunner.LineBot.WebHook.Models.ViewModels;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class GameMemberService
    {
        public async Task<int> CreateAsync(int userId, int signupId)
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var userInGame = await db.GameMember.AnyAsync(o => o.SignupId == signupId && o.UserId == userId);
                if (!userInGame)
                {
                    var gmember = new GameMember
                    {
                        SignupId = signupId,
                        UserId = userId,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow
                    };
                    db.Entry(gmember).State = EntityState.Added;
                    await db.SaveChangesAsync();

                    return gmember.Id;
                }
                return -1;
            }
        }

        public async Task<IEnumerable<User>> FindLatestGameUsersAsync()
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var now = DateTime.UtcNow;
                var game = await db.Signup.FirstOrDefaultAsync(o => o.StartTime <= now && o.EndTime >= now && o.IsEnabled == true && o.IsDeleted != true && o.IsOnGoing != true && o.IsStoryPushed != true);
                if (game != null)
                {
                    var gameMembers = db.GameMember.Where(x => x.SignupId == game.Id).Select(y => y.User);
                    return gameMembers;
                }
                else
                {
                    return new User[] { };
                }
            }
        }

        /// <summary>
        /// 遊戲中還活著的倖存者
        /// </summary>
        public async Task<GameSurvivors> FindAvailableUsersInOngoingGame()
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var now = DateTime.UtcNow;
                var game = await db.Signup.FirstOrDefaultAsync(o => (o.IsOnGoing == true || o.EndTime <= now) && o.StoryBeginTime <= now && o.StoryEndedTime >= now && o.IsEnabled == true && o.IsDeleted != true && o.IsStoryPushed != true);
                if (game != null)
                {
                    var gameMembers = db.GameMember.Where(x => x.SignupId == game.Id && x.IsDead != true).Select(y => y.User);
                    return new GameSurvivors {
                        Game = game,
                        Survivors = gameMembers
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}