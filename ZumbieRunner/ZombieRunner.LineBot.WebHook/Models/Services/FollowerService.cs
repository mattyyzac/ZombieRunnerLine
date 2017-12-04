using System;
using System.Collections.Generic;
using System.Linq;
using ZombieRunner.LineBot.WebHook.Models.Enums;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class FollowerService
    {
        public IEnumerable<User> GetFollowers()
        {
            var fmax = Cores.Numbers.Max<FollowerEnums>();
            var fs = new List<User>();
            for (int i = 0; i <= fmax; i++)
            {
                fs.Add(new User {
                    Id = 9000 + i,
                    Name = ((FollowerEnums)i).ToString()
                });
            }
            return fs.ToArray();
        }

        public IEnumerable<User> GetRndFollowers()
        {
            var fmax = Cores.Numbers.Max<FollowerEnums>();
            var rnd = new Random((int)(DateTime.UtcNow.Ticks % 9999));

            var dest = new Dictionary<int, int>();
            for (int i = 0; i < fmax; i++)
            {
                dest.Add(rnd.Next(fmax *10000), i);
            }
            var maxFollowers = rnd.Next(1, (fmax % 2 == 0 ? fmax /2 : (fmax +1) /2) +1); // 最多 n /2 位跟隨者
            var arr = dest.OrderBy(o => o.Key).Select(o => o.Value).Take(maxFollowers).ToArray();
            var fs = new List<User>();
            foreach (var item in arr)
            {
                fs.Add(new User {
                    Id = 9000 + item,
                    Name = ((FollowerEnums)item).ToString()
                });
            }
            return fs.ToArray();
        }
    }
}