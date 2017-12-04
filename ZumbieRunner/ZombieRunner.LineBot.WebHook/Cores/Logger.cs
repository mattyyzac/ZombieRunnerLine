using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace ZombieRunner.LineBot.WebHook.Cores
{
    public class Logger
    {
        public static async Task WriteErrorLog(string logData)
        {
            var logpath = SetLogPath(Cores.Strings.LineBotLogPath);
            var logName = $"error-{DateTime.UtcNow.Ticks.ToString()}-{Cores.Strings.GenRandText(7, false)}.log";
            await WriteLog(logpath, logName, logData);
        }
        public static async Task WriteInfoLog(string logData)
        {
            var logpath = SetLogPath(Cores.Strings.LineBotLogPath);
            var logName = $"{DateTime.UtcNow.Ticks.ToString()}-{Cores.Strings.GenRandText(7, false)}.log";
            await WriteLog(logpath, logName, logData);
        }

        #region base settings

        /// <summary>
        /// log folder: /logs/year/month/day
        /// </summary>
        private static string SetLogPath(string additionalPath)
        {
            var logpath = "";
            var logRoot = "~/App_Data/Logs";
            if (!string.IsNullOrEmpty(additionalPath))
            {
                logRoot = $"{logRoot}/{additionalPath}";
            }
            logpath = HostingEnvironment.MapPath("~/App_Data");

            if (string.IsNullOrEmpty(logpath))
                throw new FileNotFoundException("base log file path error!");

            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }

            var now = DateTime.UtcNow.AddHours(8);
            var y = now.Year.ToString();
            var m = now.Month.ToString();
            var d = now.Day.ToString();
            var ymd = $"{y}{m}{d}";
            var lpath = Path.Combine(logpath, ymd);
            if (!Directory.Exists(lpath))
            {
                Directory.CreateDirectory(lpath);
            }
            return lpath;
        }
        
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        /// <param name="logName">Name of the log.</param>
        /// <param name="logData">The log data.</param>
        private static async Task WriteLog(string logPath, string logName, string logData)
        {
            var logFile = Path.Combine(logPath, logName);
            using (var file = new StreamWriter(logFile, true))
            {
                try
                {
                    await file.WriteLineAsync(logData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
    }
}