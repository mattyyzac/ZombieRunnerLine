using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ZombieRunner.LineBot.WebHook.Cores
{
    public static class Numbers
    {
        public static int Max<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<int>().Max();
        }

        public static double EnumDescriptionAsWeight<TEnum>(this TEnum data)
        {
            var desc = data.EnumDescription();
            var result = 0.1;
            double.TryParse(desc, out result);
            return result;
        }

        private static string EnumDescription<TEnum>(this TEnum source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return source.ToString();
            }
        }
    }
}