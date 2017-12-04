using System;
using System.Collections.Generic;
using ZombieRunner.LineBot.WebHook.Models.Enums;

namespace ZombieRunner.LineBot.WebHook.Models.Services
{
    public class WeatherService
    {
        public IEnumerable<WeatherEnums> GetRndWeathers(int wNumber = 30)
        {
            var weathers = new List<WeatherEnums>();
            var seed = Math.Floor((double)DateTime.UtcNow.Ticks % 9999);
            var rnd = new Random((int)seed);
            for (int i = 0; i < wNumber; i++)
            {
                var rmax = Cores.Numbers.Max<WeatherEnums>();
                var r = rnd.Next(rmax);
                weathers.Add((WeatherEnums)r);
            }
            return weathers.ToArray();
        }
    }
}