using ZombieRunner.LineBot.WebHook.Models.Enums;

namespace ZombieRunner.LineBot.WebHook.Models
{
    public class Event : BaseModel
    {
        public long Seed { get; set; }
        public WeatherEnums? Weather { get; set; }
        public string Content { get; set; }
        public bool? IsAnyoneDead { get; set; }
        public string CreatedBy { get; set; }
    }
}