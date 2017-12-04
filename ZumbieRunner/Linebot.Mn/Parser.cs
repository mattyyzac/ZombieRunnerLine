using Linebot.Mn.Models.LineJsonModels;
using Newtonsoft.Json;

namespace Linebot.Mn
{
    public class Parser
    {
        public static EventResult EventParser(string jsondata)
        {
            var evt = JsonConvert.DeserializeObject<EventResult>(jsondata);
            if (evt != null)
            {
                evt.IsParseOk = true;
            }
            return evt;
        }
    }
}