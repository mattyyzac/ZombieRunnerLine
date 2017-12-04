using System.ComponentModel.DataAnnotations;

namespace ZombieRunner.LineBot.WebHook.Models
{
    public class User : BaseModel
    {
        /// <summary>
        /// line uer Id
        /// </summary>
        [MaxLength(50)]
        public string LineUserId { get; set; }
        /// <summary>
        /// 使用者的名字（當未有名字時，Bot 將引導使用者以指令填入名字）
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }
        public int? Level { get; set; }
        public int? Experience { get; set; }
        /// <summary>
        /// 參加遊戲次數
        /// </summary>
        public int? GameCounts { get; set; }
    }
}