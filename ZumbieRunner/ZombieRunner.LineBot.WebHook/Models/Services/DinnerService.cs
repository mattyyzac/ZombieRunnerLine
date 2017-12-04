using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class DinnerService
    {
        public async Task<Dinner> AddDinner(string dinner, string createdBy)
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var data = new Dinner
                {
                    Seed = new Random().Next(DateTime.UtcNow.Millisecond * 1000),
                    Content = dinner,
                    IsEnabled = true,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = createdBy,
                    ModifiedOn = DateTime.UtcNow
                };
                db.Entry(data).State = EntityState.Added;
                await db.SaveChangesAsync();

                return data;
            }
        }

        public async Task<IEnumerable<Dinner>> GetAvailableDinners()
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var dinners = await db.Dinner.Where(o => o.IsEnabled == true && o.IsDeleted != true).ToArrayAsync();
                return dinners;
            }
        }
        public async Task<IEnumerable<Dinner>> GetRndDinners(int dNumber)
        {
            var dinners = new List<Dinner>();
            var availableDinners = (await GetAvailableDinners()).ToArray();

            var rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < dNumber; i++)
            {
                var r = rnd.Next(availableDinners.Length);
                dinners.Add(availableDinners[r]);
            }
            return dinners.ToArray();
        }
    }
}