namespace ZombieRunner.LineBot.WebHook.Models
{
    public class Dinner : BaseModel
    {
        public long Seed { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
    }
}