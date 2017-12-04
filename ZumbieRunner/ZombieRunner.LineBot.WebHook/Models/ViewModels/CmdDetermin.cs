namespace ZombieRunner.LineBot.WebHook.Models.ViewModels
{
    public class CmdDetermin
    {
        /// <summary>
        /// 回應的訊息
        /// </summary>
        public string ReturnMessage { get; set; }
        /// <summary>
        ///檢查該指令是否有執行分析
        /// </summary>
        public bool Result { get; set; }
    }
}