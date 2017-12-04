using System.Collections.Generic;

namespace ZombieRunner.LineBot.WebHook.Models.ViewModels
{
    public class GameSurvivors
    {
        public Signup Game { get; set; }
        public IEnumerable<User> Survivors { get; set; }
    }
}