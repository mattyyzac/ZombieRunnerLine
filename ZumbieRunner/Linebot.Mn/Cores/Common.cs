using System.Net.Http;
using System.Net.Http.Headers;

namespace Linebot.Mn.Cores
{
    public static class Common
    {
        public static HttpClient SetHttpClientHeader(string token)
        {
            var httpClient = new HttpClient();
            //var accessToken = token; //Cores.Strings.LineBotAccessToken;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            return httpClient;
        }
    }
}