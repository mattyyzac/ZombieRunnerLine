using Linebot.Mn.Models.LineJsonModels.Enums;

namespace Linebot.Mn.Models.LineJsonModels
{
    public class ImageMapMessageObject
    {
        public class ImageMapMessage : IMessage
        {
            public string type { get; set; }
            public string baseUrl { get; set; }
            public string altText { get; set; }
            public BaseSize baseSize { get; set; }
            public ITemplateAction[] actions { get; set; }

            public ImageMapMessage()
            {
                this.type = MessageTypeEnum.imagemap.ToString();
            }
        }
        public class BaseSize
        {
            public int width { get; set; }
            public int height { get; set; }
        }
    }
}