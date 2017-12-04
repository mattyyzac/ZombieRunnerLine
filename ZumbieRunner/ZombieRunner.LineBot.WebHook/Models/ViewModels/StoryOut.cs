using System.Collections.Generic;

namespace ZombieRunner.LineBot.WebHook.Models.ViewModels
{
    public class StoryOut
    {
        /// <summary>
        /// 有加入遊戲的使用者 UserId。會收到劇情故事訊息推播的人。
        /// </summary>
        public IEnumerable<string> Survivors { get; set; }
        /// <summary>
        /// 故事劇情
        /// </summary>
        public IEnumerable<string> Stories { get; set; }
    }
}