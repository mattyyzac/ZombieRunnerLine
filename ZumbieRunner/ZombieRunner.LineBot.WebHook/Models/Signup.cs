using System;
using System.Collections.Generic;

namespace ZombieRunner.LineBot.WebHook.Models
{
    public class Signup : BaseModel
    {
        /// <summary>
        /// 開團主持人 lineUserId
        /// </summary>
        public string HostUserId { get; set; }
        /// <summary>
        /// 提出報名時的整段對話
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 報名開始時間
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 報名截止時間
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 遊戲是否進行中（此時不能報名）
        /// </summary>
        public bool? IsOnGoing { get; set; }
        /// <summary>
        /// 遊戲進行時間，預設是報名開始的第 31 分鐘。主持人可以提前強迫開始
        /// </summary>
        public DateTime? StoryBeginTime { get; set; }
        /// <summary>
        /// 遊戲結束時間，報名開始 +65 分鐘
        /// </summary>
        public DateTime? StoryEndedTime { get; set; }
        /// <summary>
        /// 故事是否已經推送
        /// </summary>
        public bool? IsStoryPushed { get; set; }

        public virtual ICollection<GameMember> GameMembers { get; set; }
    }
}