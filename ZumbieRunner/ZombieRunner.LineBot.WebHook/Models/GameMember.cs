using System.ComponentModel.DataAnnotations.Schema;

namespace ZombieRunner.LineBot.WebHook.Models
{
    public class GameMember : BaseModelWithoutStatus
    {
        /// <summary>
        /// 報名資訊
        /// </summary>
        [ForeignKey("Signup")]
        public int SignupId { get; set; }
        public virtual Signup Signup { get; set; }
        /// <summary>
        /// 參人員
        /// </summary>
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public bool? IsDead { get; set; }
    }
}