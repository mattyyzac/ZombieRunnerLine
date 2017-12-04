using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class UserService
    {
        public async Task<int> CreateUserAsync(string lineUserId)
        {
            if (string.IsNullOrEmpty(lineUserId))
                return -1;

            using (var db = new ZombieRunnerDbContex())
            {
                if (!db.User.Any(o => o.LineUserId == lineUserId))
                {
                    var newUser = new User
                    {
                        LineUserId = lineUserId,
                        Name = string.Empty,
                        IsEnabled = true,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow
                    };
                    db.Entry(newUser).State = EntityState.Added;
                    await db.SaveChangesAsync();

                    return newUser.Id;
                }
                return 0;
            }
        }

        public async Task UpdateUserNameAsync(string lineUserId, string name)
        {
            if (string.IsNullOrEmpty(lineUserId) || string.IsNullOrEmpty(name))
                return;

            using (var db = new ZombieRunnerDbContex())
            {
                var user = await db.User.FirstOrDefaultAsync(o => o.LineUserId == lineUserId);
                if (user != null)
                {
                    user.Name = name;
                    user.ModifiedOn = DateTime.UtcNow;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// 啟用/停用使用者帳戶
        /// </summary>
        /// <param name="lineUserId">The line user identifier.</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <returns></returns>
        public async Task EnableUserAsync(string lineUserId, bool isEnabled)
        {
            if (string.IsNullOrEmpty(lineUserId)) return;

            using (var db = new ZombieRunnerDbContex())
            {
                var user = await db.User.FirstOrDefaultAsync(o => o.LineUserId == lineUserId);
                if (user != null)
                {
                    user.IsEnabled = isEnabled;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
        }
        
        public async Task<User> GetUserByLineUserIdAsync(string lineUserId)
        {
            if (string.IsNullOrEmpty(lineUserId))
                return null;

            using (var db = new ZombieRunnerDbContex())
            {
                var user = await db.User.FirstOrDefaultAsync(o => o.LineUserId == lineUserId);
                return user;
            }
        }

        public async Task<User> GetAvailableUserByLineUserIdAsync(string lineUserId)
        {
            if (string.IsNullOrEmpty(lineUserId))
                return null;

            using (var db = new ZombieRunnerDbContex())
            {
                var user = await db.User.FirstOrDefaultAsync(o => o.LineUserId == lineUserId && o.IsEnabled == true && o.IsDeleted != true);
                return user;
            }
        }

        public async Task<IEnumerable<User>> GetAvailableUsers()
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var users = await db.User.Where(o => o.IsEnabled == true && o.IsDeleted != true).ToArrayAsync();
                return users;
            }
        }
    }
}