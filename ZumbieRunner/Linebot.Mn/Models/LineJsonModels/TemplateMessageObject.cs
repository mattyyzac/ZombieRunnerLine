using Linebot.Mn.Models.LineJsonModels.Enums;

namespace Linebot.Mn.Models.LineJsonModels
{
    public class TemplateMessage : ITemplateMessage
    {
        public string type { get; set; }
        /// <summary>
        /// Max: 400 characters
        /// </summary>
        public string altText { get; set; }
        /// <summary>
        /// Template messages are messages with predefined layouts which you can customize. There are three types of templates available that can be used to interact with users through your bot.
        /// </summary>
        public ITemplateMessageType template { get; set; }
        public TemplateMessage()
        {
            this.type = MessageTypeEnum.template.ToString();
        }
    }

    /// <summary>
    /// Template message with an image, title, text, and multiple action buttons.
    /// </summary>
    public class ButtonTemplateType : ITemplateMessageType
    {
        public string type { get; set; }
        /// <summary>
        /// 	Image URL (Max: 1000 characters)
        /// 	HTTPS
        /// 	JPEG or PNG
        /// 	Aspect ratio: 1:1.51
        /// 	Max width: 1024px
        /// 	Max: 1 MB
        /// </summary>
        public string thumbnailImageUrl { get; set; }
        public string title { get; set; }
        /// <summary>
        /// Max: 160 characters (no image or title)
        /// Max: 60 characters(message with an image or title)
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// Action when tapped
        /// Max: 4
        /// </summary>
        public ITemplateAction[] actions { get; set; }
        public ButtonTemplateType()
        {
            this.type = TemplateMessgeTypeEnum.buttons.ToString();
        }
    }

    /// <summary>
    /// Template message with two action buttons.
    /// </summary>
    public class ConfirmTemplateType : ITemplateMessageType
    {
        public string type { get; set; }
        /// <summary>
        /// Max: 240 characters
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// Action when tapped
        /// Max: 2
        /// </summary>
        public ITemplateAction[] actions { get; set; }
        public ConfirmTemplateType()
        {
            this.type = TemplateMessgeTypeEnum.confirm.ToString();
        }
    }

    /// <summary>
    /// Template message with multiple columns which can be cycled like a carousel.
    /// </summary>
    public class CarouselTemplateType : ITemplateMessageType
    {
        public string type { get; set; }
        /// <summary>
        /// Array of columns
        /// Max: 10
        /// </summary>
        public IColumnObject[] columns { get; set; }

        public string imageAspectRatio { get; set; }
        public string imageSize { get; set; }

        public CarouselTemplateType()
        {
            this.type = TemplateMessgeTypeEnum.carousel.ToString();
        }
    }

    public enum imageAspectRatioEnum
    {
        rectangle,
        square
    }

    public enum imageSizeEnum
    {
        cover,
        contain
    }

    public class CarouselColumObject : IColumnObject
    {
        /// <summary>
        /// Image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG or PNG
        /// Aspect ratio: 1:1.51
        /// Max width: 1024px
        /// Max: 1 MB
        /// </summary>
        public string thumbnailImageUrl { get; set; }
        /// <summary>
        /// Background color of image. Specify a RGB color value. The default value is #FFFFFF (white).
        /// </summary>
        public string imageBackgroundColor { get; set; }
        /// <summary>
        /// Max: 40 characters
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Required
        /// Message text
        /// Max: 120 characters(no image or title)
        /// Max: 60 characters(message with an image or title)
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// Required
        /// Action when tapped
        /// Max: 3
        /// </summary>
        public ITemplateAction[] actions { get; set; }
    }

    public interface IColumnObject
    {
        string text { get; set; }
        ITemplateAction[] actions { get; set; }
    }

    public interface ITemplateMessageType
    {
        string type { get; set; }
    }

    public interface ITemplateMessage : IMessage
    {
        string altText { get; set; }
    }
}
