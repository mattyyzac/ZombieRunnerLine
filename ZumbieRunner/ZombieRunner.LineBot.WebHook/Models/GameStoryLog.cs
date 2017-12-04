using System.ComponentModel.DataAnnotations.Schema;

namespace ZombieRunner.LineBot.WebHook.Models
{
    public class GameStoryLog : BaseModel
    {
        [ForeignKey("Signup")]
        public int SignupId { get; set; }
        public virtual Signup Signup { get; set; }
        public int DayNumber { get; set; }
        /// <summary>
        /// 故事完整內容
        /// </summary>
        public string Detail { get; set; }
    }
}