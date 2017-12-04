using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieRunner.LineBot.WebHook.Models;
using ZombieRunner.LineBot.WebHook.Models.ViewModels;
using ZombieRunner.LineBot.WebHook.Models.Services;

namespace ZombieRunner.LineBot.WebHook.Cores
{
    public class StoryMaker
    {

        #region service def

        private readonly GameMemberService _gameMemberService;
        private readonly EventService _eventService;
        private readonly WeatherService _weatherService;
        private readonly FollowerService _followerService;
        private readonly DinnerService _dinnerService;
        private readonly SignupService _signupService;

        public StoryMaker()
        {
            this._gameMemberService = new GameMemberService();
            this._eventService = new EventService();
            this._weatherService = new WeatherService();
            this._followerService = new FollowerService();
            this._dinnerService = new DinnerService();
            this._signupService = new SignupService();
        }
        #endregion

        public async Task<StoryOut> Go()
        {
            var cycleDays = 30;
            var stories = new List<string>();
            var gsurvivors = await this._gameMemberService.FindAvailableUsersInOngoingGame();
            if (gsurvivors == null || !gsurvivors.Survivors.Any())
            {
                return new StoryOut
                {
                    Survivors = new[] { "" },
                    Stories = new[] { "" }
                };
            }

            var followers = this._followerService.GetRndFollowers();
            var people = JoinGroup(gsurvivors.Survivors, followers);
            var weathers = this._weatherService.GetRndWeathers(cycleDays).ToArray();
            var dinners = (await this._dinnerService.GetRndDinners(cycleDays)).ToArray();

            if (gsurvivors == null || !gsurvivors.Survivors.Any() || !people.Any())
            {
                stories.Add("沒有人生還...");
                return new StoryOut {
                    Survivors = new[] { "" },
                    Stories = new[] { "" }
                };
            }

            var evRnd = new Random(DateTime.UtcNow.Millisecond);
            for (int d = 0; d < cycleDays; d++)
            {
                var sbStory = new StringBuilder();

                sbStory.Append($"第 {d+1} 天，{weathers[d].ToString()}\n");

                var pevents = await EventsGenerater(evRnd, people);
                people = pevents.People;

                sbStory.Append(pevents.EventsContext);
                if (people.Any())
                {
                    sbStory.Append(dinners[d].Content);
                }
                stories.Add(sbStory.ToString());

                if (!people.Any())
                {
                    stories.Add("所有人全都死光光了...");
                    break;
                }
                else
                {
                    if (d == 29)
                    {
                        var left = people.Select(o => o.Name).Aggregate((result, u) => result + "、" + u);
                        stories.Add($"{left}一伙人總算逃出殭屍無情的追殺了。");
                    }
                }
            }

            if (gsurvivors.Survivors.Any())
                await this._signupService.ResetToEndAsync(gsurvivors.Game);

            return new StoryOut {
                Survivors = gsurvivors.Survivors.Select(o => o.LineUserId),
                Stories = stories
            };
        }

        public async Task<StoryOut> Demo()
        {
            var cycleDays = 30;
            var stories = new List<string>();
            var followers = this._followerService.GetRndFollowers();
            var weathers = this._weatherService.GetRndWeathers(cycleDays).ToArray();
            var dinners = (await this._dinnerService.GetRndDinners(cycleDays)).ToArray();

            var evRnd = new Random(DateTime.UtcNow.Millisecond);
            for (int d = 0; d < cycleDays; d++)
            {
                var sbStory = new StringBuilder();

                sbStory.Append($"第 {d + 1} 天，{weathers[d].ToString()}\n");

                var pevents = await EventsGenerater(evRnd, followers);
                followers = pevents.People;

                sbStory.Append(pevents.EventsContext);
                if (followers.Any())
                {
                    sbStory.Append(dinners[d].Content);
                }
                stories.Add(sbStory.ToString());

                if (!followers.Any())
                {
                    stories.Add("所有人全都死光光了...");
                    break;
                }
                else
                {
                    if (d == 29)
                    {
                        var left = followers.Select(o => o.Name).Aggregate((result, u) => result + "、" + u);
                        stories.Add($"{left}一伙人總算逃出殭屍無情的追殺了。");
                    }
                }
            }
            
            return new StoryOut
            {
                Survivors = new[] { "" },
                Stories = stories
            };
        }

        private async Task<PEvents> EventsGenerater(Random seed, IEnumerable<User> people)
        {
            var events = (await this._eventService.GetAvailableEvents()).ToArray();

            // 由參與人數控制事件數量
            var evmax = seed.Next(1, people.Count());
            var eventPieces = seed.Next(1, evmax);
            var sbEvents = new StringBuilder();
            for (int e = 0; e < eventPieces; e++)
            {
                var ev = events[seed.Next(events.Length)];
                var someone = GetRndSurvivor(people.ToArray());
                if (someone == null) break;

                if (ev.IsAnyoneDead == true)
                {
                    people = people.Where(o => o.Id != someone.Id).ToArray();
                }
                var evContent = ev.Content.Replace("{0}", $"【{someone.Name}】");
                sbEvents.Append(evContent + "\n");

                events = events.Where(x => x.Id != ev.Id).ToArray();
            }
            //return sbEvents.ToString();
            return new PEvents {
                EventsContext = sbEvents.ToString(),
                People = people
            };
        }

        public class PEvents
        {
            public string EventsContext { get; set; }
            public IEnumerable<User> People { get; set; }
        }

        private IEnumerable<User> JoinGroup(IEnumerable<User> a, IEnumerable<User> b)
        {
            var group = new List<User>();
            group.AddRange(a);
            group.AddRange(b);
            return group.ToArray();
        }

        private User GetRndSurvivor(User[] people)
        {
            if (people.Length <= 0) return null;

            var rnd = new Random(DateTime.UtcNow.Millisecond);
            var r = rnd.Next(people.Length);

            return people[r];
        }
    }
}