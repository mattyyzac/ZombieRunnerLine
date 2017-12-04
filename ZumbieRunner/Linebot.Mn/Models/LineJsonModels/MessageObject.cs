using Linebot.Mn.Models.LineJsonModels.Enums;

namespace Linebot.Mn.Models.LineJsonModels
{
    /// <summary>
    /// line text mesage
    /// </summary>
    public class TextMessage : IMessage
    {
        public string type { get; set; }
        /// <summary>
        /// Max: 2000 characters
        /// </summary>
        public string text { get; set; }

        public TextMessage()
        {
            this.type = MessageTypeEnum.text.ToString();
        }
    }

    public class ImageMessage: IMessage
    {
        public string type { get; set; }
        /// <summary>
        /// 	Image URL (Max: 1000 characters)
        /// 	HTTPS
        /// 	JPEG
        /// 	Max: 1024 x 1024
        /// 	Max: 1 MB
        /// </summary>
        public string originalContentUrl { get; set; }
        /// <summary>
        /// 	Image URL (Max: 1000 characters)
        /// 	HTTPS
        /// 	JPEG
        /// 	Max: 1024 x 1024
        /// 	Max: 1 MB
        /// </summary>
        public string previewImageUrl { get; set; }

        public ImageMessage()
        {
            this.type = MessageTypeEnum.image.ToString();
        }
    }

    public class VideoMessage: IMessage
    {
        public string type { get; set; }
        /// <summary>
        /// URL of video file (Max: 1000 characters)
        /// HTTPS
        /// mp4
        /// Less than 1 minute
        /// Max: 10 MB
        /// </summary>
        public string originalContentUrl { get; set; }
        /// <summary>
        /// URL of preview image (Max: 1000 characters)
        /// HTTPS
        /// JPEG
        /// Max: 240 x 240
        /// Max: 1 MB
        /// </summary>
        public string previewImageUrl { get; set; }

        public VideoMessage()
        {
            this.type = MessageTypeEnum.video.ToString();
        }
    }

    public class AudioMessage : IMessage
    {
        public string type { get; set; }
        /// <summary>
        /// URL of audio file (Max: 1000 characters)
        /// HTTPS
        /// m4a
        /// Less than 1 minute
        /// Max 10 MB
        /// </summary>
        public string originalContentUrl { get; set; }
        /// <summary>
        /// Length of audio file (milliseconds)
        /// </summary>
        public int duration { get; set; }
    }

    public class LocationMessage: IMessage
    {
        public string type { get; set; }
        /// <summary>
        /// Max: 100 characters
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// Max: 100 characters
        /// </summary>
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public LocationMessage()
        {
            this.type = MessageTypeEnum.location.ToString();
        }
    }

    /// <summary>
    /// For a list of the sticker IDs for stickers that can be sent with the Messaging API
    /// see Sticker list: https://devdocs.line.me/files/sticker_list.pdf
    /// </summary>
    public class StickerMessage: IMessage
    {
        public string type { get; set; }
        public string packageId { get; set; }
        public string stickerId { get; set; }

        public StickerMessage()
        {
            this.type = MessageTypeEnum.sticker.ToString();
        }
    }

    public interface IMessage
    {
        string type { get; set; }
    }
}