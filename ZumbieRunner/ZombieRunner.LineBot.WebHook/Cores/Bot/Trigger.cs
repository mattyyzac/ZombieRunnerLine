using System;
using System.Linq;
using System.Threading.Tasks;
using ZombieRunner.LineBot.WebHook.Models.Services;
using lb = Linebot.Mn;
using lm = Linebot.Mn.Models.LineJsonModels;

namespace ZombieRunner.LineBot.WebHook.Cores.Bot
{
    /// <summary>
    /// 分析使用者傳送給 Bot 的訊息物件
    /// </summary>
    public class Trigger
    {
        #region service def

        private readonly UserService _userService;
        private readonly SignupService _signupService;
        private readonly GameMemberService _gameMemberService;
        private readonly EventService _eventService;
        private readonly DinnerService _dinnerService;

        private readonly Atrigger _atrigger;

        public Trigger()
        {
            this._userService = new UserService();
            this._signupService = new SignupService();
            this._gameMemberService = new GameMemberService();
            this._eventService = new EventService();
            this._dinnerService = new DinnerService();

            this._atrigger = new Atrigger();
        }
        #endregion

        public async Task<string> Read(lm.EventResult message)
        {
            const string defaultAnswer = "啊... 啊...（笑）";
            var answer = defaultAnswer;
            var currentEvent = message.events.FirstOrDefault();
            if (currentEvent != null)
            {
                var userId = currentEvent.source.userId;
                await this._userService.CreateUserAsync(userId);

                if (currentEvent.type == lm.Enums.EventTypeEnum.message)
                {
                    answer = await Analysis(userId, currentEvent.message.text);
                }
                else if (currentEvent.type == lm.Enums.EventTypeEnum.follow
                    || currentEvent.type == lm.Enums.EventTypeEnum.join)
                {
                    answer = await Greetings(userId);
                }
                else if (currentEvent.type == lm.Enums.EventTypeEnum.unfollow)
                {
                    await this._userService.EnableUserAsync(userId, false);
                }
                else
                {
                    answer = $"{defaultAnswer}\n{lm.Enums.EventTypeEnum.undefined.ToString()}";
                }
            }
            return answer;
        }

        private async Task<string> Analysis(string lineUserId, string messageText)
        {
            if (string.IsNullOrEmpty(lineUserId) || string.IsNullOrEmpty(messageText))
                return string.Empty;

            messageText = await NameCmdDetermine(lineUserId, messageText);
            messageText = await ListCmdDetermine(lineUserId, messageText);
            messageText = await GameStartCmdDetermine(lineUserId, messageText);
            messageText = await AddDinnerCmdDetermine(lineUserId, messageText);
            messageText = await AddEventCmdDetermine(lineUserId, messageText);
            messageText = await AddDeathEventCmdDetermin(lineUserId, messageText);
            messageText = await StartupCmdDetermine(lineUserId, messageText);
            messageText = await JoinCmmdDermine(lineUserId, messageText);

            return messageText;
        }

        #region commands determine

        private async Task<string> NameCmdDetermine(string lineUserId, string messageText)
        {
            const string NAME_CMD_NAME = "name";
            if (messageText.StartsWith($"-{NAME_CMD_NAME}")
                || messageText.StartsWith($"/{NAME_CMD_NAME}"))
            {
                var len = NAME_CMD_NAME.Length +2;
                if (messageText.Length > len)
                {
                    var newName = messageText.Substring(len).Trim();
                    await this._userService.UpdateUserNameAsync(lineUserId, newName);
                    return $"hi {newName} 哩賀...";
                }
                else
                {
                    return $"無法設定名字。";
                }
            }
            return messageText;
        }

        private async Task<string> ListCmdDetermine(string lineUserId, string messageText)
        {
            const string LIST_CMD_NAME = "list";
            const string LIST_CMD_NAME2 = "lst";
            const string LIST_CMD_NAME3 = "ls";
            if (messageText.StartsWith($"-{LIST_CMD_NAME}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{LIST_CMD_NAME}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"-{LIST_CMD_NAME2}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{LIST_CMD_NAME2}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"-{LIST_CMD_NAME3}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{LIST_CMD_NAME3}", StringComparison.OrdinalIgnoreCase))
            {
                var gameMembers = await this._gameMemberService.FindLatestGameUsersAsync();
                if (gameMembers.Any())
                {
                    var usersList = gameMembers.Select(o => o.Name).Aggregate((result, o) => result + "\n" + o);
                    return $"目前有-參-加-的玩家\n{usersList}";
                }
                else
                {
                    return "目前沒有-開-團-，或無人參加。";
                }
            }
            return messageText;
        }

        private async Task<string> GameStartCmdDetermine(string lineUserId, string messageText)
        {
            const string GO_CMD_NAME = "go";
            if (messageText.StartsWith($"-{GO_CMD_NAME}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{GO_CMD_NAME}", StringComparison.OrdinalIgnoreCase))
            {
                var cgame = await this._signupService.FindLatestForSignupAsync();
                if (cgame != null && cgame.HostUserId == lineUserId)
                {
                    await this._signupService.SetToBeginAsync(cgame);

                    this._atrigger.CreateTaskByForce();
                    System.Threading.Thread.Sleep(2000); // if go next to return without sleep, Atrigger won't create task

                    return $"遊戲-開-始-了。倖存者們，-塊-陶-啊...";
                }
                else
                {
                    return $"目前沒有遊戲或您不是遊戲主持人。";
                }
            }
            return messageText;
        }

        private async Task<string> AddDinnerCmdDetermine(string lineUserId, string messageText)
        {
            const string ADD_DINNER_CMD_NAME = "addDinner";
            if (messageText.StartsWith($"-{ADD_DINNER_CMD_NAME}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{ADD_DINNER_CMD_NAME}", StringComparison.OrdinalIgnoreCase))
            {
                var len = ADD_DINNER_CMD_NAME.Length + 2;
                if (messageText.Length > len)
                {
                    var dinner = messageText.Substring(len).Trim();
                    await this._dinnerService.AddDinner(dinner,lineUserId);
                    return $"晚餐\n{dinner}\n新增完成。";
                }
                else
                {
                    return "晚餐內容不足，無法新增。";
                }
            }
            return messageText;
        }

        private async Task<string> AddEventCmdDetermine(string lineUserId, string messageText)
        {
            const string ADD_EVENT_CMD_NAME = "addEvent";
            if (messageText.StartsWith($"-{ADD_EVENT_CMD_NAME}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{ADD_EVENT_CMD_NAME}", StringComparison.OrdinalIgnoreCase))
            {
                var len = ADD_EVENT_CMD_NAME.Length +2;
                if (messageText.Length > len)
                {
                    var ev = messageText.Substring(len).Trim();
                    await this._eventService.AddEvents(ev, false, lineUserId);
                    return $"事件\n{ev}\n新增完成。";
                }
                else
                {
                    return "事件內容不足，無法新增。";
                }
            }
            return messageText;
        }

        private async Task<string> AddDeathEventCmdDetermin(string lineUserId, string messageText)
        {
            const string ADD_DEATH_EVENT_CMD_NAME = "addDeathEvent";
            if (messageText.StartsWith($"-{ADD_DEATH_EVENT_CMD_NAME}", StringComparison.OrdinalIgnoreCase)
                || messageText.StartsWith($"/{ADD_DEATH_EVENT_CMD_NAME}", StringComparison.OrdinalIgnoreCase))
            {
                var len = ADD_DEATH_EVENT_CMD_NAME.Length +2;
                if (messageText.Length > len)
                {
                    var ev = messageText.Substring(len).Trim();
                    await this._eventService.AddEvents(ev, true, lineUserId);
                    return $"死亡事件\n{ev}\n新增完成。";
                }
                else
                {
                    return "事件內容不足，無法新增。";
                }
            }
            return messageText;
        }

        private async Task<string> StartupCmdDetermine(string lineUserId, string messageText)
        {
            if (messageText.Contains("開團"))
            {
                var desc = messageText;
                var signupId = await this._signupService.CreateAsync(lineUserId, desc);
                if (signupId > 0)
                {
                    await AddToGameMemberAsync(lineUserId, signupId);

                    this._atrigger.CreateTask();
                    System.Threading.Thread.Sleep(2000); // if go next to return without sleep, Atrigger won't create task

                    return $"【倖存者日記】第 {signupId} 團-開-團-受理！！\n@{DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd mm:hh")}";
                }
                else
                {
                    return $"【倖存者日記】目前已有-開-團-，請考慮直接【+1】參加遊戲。\n@{DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd mm:hh")}";
                }
            }
            return messageText;
        }

        private async Task<string> JoinCmmdDermine(string lineUserId, string messageText)
        {
            if ((messageText.StartsWith("+1") || messageText.Contains("我要玩")
                || messageText.Contains("加入") || messageText.Contains("報名")))
            {
                var game = await this._signupService.FindLatestForSignupAsync();
                if (game != null)
                {
                    await AddToGameMemberAsync(lineUserId, game.Id);
                    return $"您已經-加-入-遊戲...";
                }
                else
                {
                    return $"沒有人-開-團-唷！";
                }
            }
            return messageText;
        }
        #endregion

        #region actions

        private async Task<string> Greetings(string lineUserId)
        {
            var answer = string.Empty;
            var user = await this._userService.GetUserByLineUserIdAsync(lineUserId);
            if (user != null)
            {
                answer = $"hi {user.Name} 哩賀！\n。感謝參加 偽倖存者日記，這是實驗性質的玩樂 Bot。\n招呼不周，請多多指教。\n謝謝。";
            }
            else
            {
                var profile = await new lb.Retriever().GetUserProfile(lineUserId);
                if (profile != null)
                {
                    answer = $"hi {profile.displayName} 感謝參加 偽倖存者日記，這是實驗性質的玩樂 Bot。\n招呼不周，請多多指教。\n謝謝。\n\n也請告訴我你是哪位？\n【-name 你的稱呼】";
                }
            }
            return answer;
        }

        private async Task AddToGameMemberAsync(string lineUserId, int signupId)
        {
            var cuser = await this._userService.GetAvailableUserByLineUserIdAsync(lineUserId);
            if (cuser != null)
            {
                await this._gameMemberService.CreateAsync(cuser.Id, signupId);
            }
        }

        public async Task SendGamingMessage(string lineUserId)
        {
            var cuser = await this._userService.GetAvailableUserByLineUserIdAsync(lineUserId);
            if (cuser != null)
            {
                var users = await this._userService.GetAvailableUsers();
                var userIds = users.Select(o => o.LineUserId).ToArray();

                await new lb.Engine(userIds).SendTextMessage(new[] { $"\n{cuser.Name} -開-團-了，請使用 【+1】 -加-入-遊戲。" });
            }
        }
        #endregion
    }
}