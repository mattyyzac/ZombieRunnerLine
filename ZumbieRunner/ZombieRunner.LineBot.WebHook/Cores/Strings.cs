using System;
using System.Text;
using System.Web.Configuration;

namespace ZombieRunner.LineBot.WebHook.Cores
{
    public class Strings
    {
        public static string LineBotLogPath = GetAppSetting("LineBotLogPath");
        public static string ATriggerApiKey = GetAppSetting("ATriggerApiKey");
        public static string ATriggerApiSecret = GetAppSetting("ATriggerApiSecret");
        public static int StoryCarouselMaxLen = Convert.ToInt32(GetAppSetting("StoryCarouselMaxLen"));

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="section">The section.</param>
        public static string GetAppSetting(string section)
        {
            var dest = WebConfigurationManager.AppSettings[section] ?? string.Empty;
            return dest;
        }

        /// <summary>
        /// Gens the rand text.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="isUpperCaseOnly">if set to <c>true</c> [is upper case only].</param>
        public static string GenRandText(int length, bool? isUpperCaseOnly)
        {
            var possibilityStrings = (isUpperCaseOnly == true)
                ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
                : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

            var poolLength = possibilityStrings.Length;
            var rnd = new Random(DateTime.UtcNow.Millisecond);
            var randTxt = new StringBuilder(length);
            for (var l = 0; l < length; l++)
            {
                randTxt.Append(possibilityStrings.Substring(rnd.Next(poolLength), 1));
            }
            return randTxt.ToString();
        }
    }
}