using System.ComponentModel;

namespace ZombieRunner.LineBot.WebHook.Models.Enums
{
    public enum WeatherEnums
    {
        [Description(".8")]
        天氣晴,
        [Description(".6")]
        陰陰的,
        [Description(".5")]
        飄著細雨,
        [Description(".4")]
        暴雨不斷下著,
        [Description(".3")]
        閃電和暴雨,
        [Description(".2")]
        天空下雪了,
        [Description(".4")]
        陰暗的天空,
        [Description(".2")]
        颱風襲來,
        [Description(".1")]
        霧霾嚴重
    }
}