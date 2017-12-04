using ATriggerLib;
using System.Collections.Generic;

namespace ZombieRunner.LineBot.WebHook.Cores
{
    public class Atrigger
    {

        public string SendStoryApiUrl { get; private set; }

        public Atrigger()
        {
            this.SendStoryApiUrl = "https://zzombierunner.azurewebsites.net/api/bot/sendstory";
        }

        public void CreateTask()
        {
            // new game startup. OnGoing = false
            DoCreate(32);
        }

        private void DoCreate(int timeValue)
        {
            ATrigger.Initialize(Cores.Strings.ATriggerApiKey, Cores.Strings.ATriggerApiSecret);
            var tags = new Dictionary<string, string>
            {
                { "type", Cores.Strings.GenRandText(11, false) }
            };
            ATrigger.Client.doCreate(TimeQuantity.Minute(), timeValue.ToString(), this.SendStoryApiUrl, tags);
        }

        public void CreateTaskByForce()
        {
            // a game set to begin force. OnGoing = true
            DoCreate(2);
        }
    }
}