using Linebot.Mn.Models.LineJsonModels;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Linebot.Mn
{
    public class Retriever
    {
        public async Task<ProfileObject> GetUserProfile(string lineUserId)
        {
            var client = Cores.Common.SetHttpClientHeader(Cores.Strings.LineBotAccessToken);
            var url = $"https://api.line.me/v2/bot/profile/{lineUserId}";
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<ProfileObject>(result);
            return profile;
        }

        public async Task<byte[]> GetContent(string messageId)
        {
            var client = Cores.Common.SetHttpClientHeader(Cores.Strings.LineBotAccessToken);
            var url = $"https://api.line.me/v2/bot/message/{messageId}/content";
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsByteArrayAsync();
            return result;
        }
    }
}