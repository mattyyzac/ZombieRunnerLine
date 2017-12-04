using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ZombieRunner.LineBot.WebHook.Models.Enums;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class EventService
    {
        public async Task<Event[]> GetAvailableEvents()
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var evs = await db.Event.Where(o => o.IsEnabled == true && o.IsDeleted != true).ToArrayAsync();
                return evs;
            }
        }

        public async Task<IEnumerable<Event>> GetRndEvents(WeatherEnums? weather, int eNumber)
        {
            var events = new List<Event>();
            var availableEvents = await GetAvailableEvents();
            var evArrays = availableEvents.ToArray();
            if (weather.HasValue)
            {
                evArrays = availableEvents.Where(o => o.Weather == weather).ToArray();
            }
            var rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < eNumber *3; i++)
            {
                var q = rnd.Next(availableEvents.ToArray().Length);
                events.Add(evArrays[q]);
            }
            return events.ToArray();
        }

        public async Task<Event> AddEvents(string content, bool hasDead, string createdBy)
        {
            using (var db = new ZombieRunnerDbContex())
            {
                var ev = new Event {
                    Seed = new Random().Next(DateTime.UtcNow.Millisecond * 1000),
                    Content = content,
                    IsEnabled = true,
                    IsAnyoneDead = hasDead,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = createdBy,
                    ModifiedOn = DateTime.UtcNow
                };
                db.Entry(ev).State = EntityState.Added;
                await db.SaveChangesAsync();

                return ev;
            }
        }
    }
}