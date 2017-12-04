using Linebot.Mn.Models.LineJsonModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ZombieRunner.LineBot.WebHook.Models.Services;
using lb = Linebot.Mn;
using System.Collections.Generic;

namespace ZombieRunner.LineBot.WebHook.Controllers
{
    public class BotController : ApiController
    {
        #region service def

        private readonly UserService _userService;
        private readonly SignupService _signupService;
        private readonly WeatherService _weatherService;
        private readonly EventService _eventService;
        private readonly GameMemberService _gameMemberService;
        private readonly FollowerService _followerService;

        private readonly Cores.StoryMaker _storyMaker;

        public BotController()
        {
            this._userService = new UserService();
            this._signupService = new SignupService();
            this._weatherService = new WeatherService();
            this._eventService = new EventService();
            this._gameMemberService = new GameMemberService();
            this._followerService = new FollowerService();

            this._storyMaker = new Cores.StoryMaker();
        }
        #endregion

        [HttpGet]
        //public async Task<IHttpActionResult> Test()
        public IHttpActionResult Test()
        {
            return Json(new { });
        }
        
        [HttpGet]
        public async Task SendStory()
        {
            await LongTermPushing();
        }

        private async Task LongTermPushing()
        {
            var st = await new Cores.StoryMaker().Go();
            if (st.Survivors.Any() && st.Stories.Any())
            {
                var carouselMax = Cores.Strings.StoryCarouselMaxLen;
                foreach (var story in st.Stories)
                {
                    var e = new lb.Engine(st.Survivors);
                    var msg = new List<TemplateMessage>();

                    if (story.Length <= carouselMax)
                    {
                        msg.Add(Cores.Bot.MessageBuilder.GenConfirmTemplateMessge(story));
                    }
                    else
                    {
                        msg.Add(Cores.Bot.MessageBuilder.GenCarouselTemplateMessage(story));
                    }
                    await e.SendTemplateMessage(msg);
                    System.Threading.Thread.Sleep(60000);
                }
            }
        }

        private async Task SendTextWithEmoji()
        {
            var e = new lb.Engine(new[] { "U78ff37f122930697fd348c9a77a742be" });
            await e.SendTextMessage(new[] { @"在搜索百貨公司時，遭遇了大量的殭屍，所幸消耗大量彈藥後成功逃走了。 晚餐吃了玉米糊。\n" });
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> Hook()
        {
            var data = await GrabData();
            await Response(data);
            return Ok();
        }

        private async Task<string> GrabData()
        {
            var postData = Request.Content.ReadAsStringAsync().Result;
            await Cores.Logger.WriteInfoLog(postData);
            return postData;
        }

        private async Task Response(string data)
        {
            var evtData = lb.Parser.EventParser(data);
            if (evtData != null && evtData.IsParseOk == true && evtData.events.Any())
            {
                var destUserId = evtData.events.Any() ? evtData.events.FirstOrDefault().source.userId : string.Empty;
                var ans = await new Cores.Bot.Trigger().Read(evtData);
                var userIds = new[] { destUserId };
                var e = new lb.Engine(userIds);
                var messages = new List<IMessage> {
                    new TextMessage { text = $"{ans}" },
                    new StickerMessage { packageId = "1", stickerId = "2" }
                };
                await e.SendMultipleMessages(messages);

                if (ans.Contains("開團受理"))
                {
                    await new Cores.Bot.Trigger().SendGamingMessage(destUserId);
                }
            }
            else
            {
                await Cores.Logger.WriteErrorLog("webhook event objects parse ERROR!");
            }
        }

        /// <summary>
        /// Profiles the specified identifier.
        /// </summary>
        [HttpGet]
        public async Task<IHttpActionResult> Profile(string id)
        {
            var data = await new lb.Retriever().GetUserProfile(id);
            return Json(data);
        }
    }
}
