using Linebot.Mn.Models.LineJsonModels.Enums;

namespace Linebot.Mn.Models.LineJsonModels
{
    /// <summary>
    /// When this action is tapped, a postback event is returned via webhook with the specified string in the data field.
    /// If you have included the text field, the string in the text field is sent as a message from the user.
    /// </summary>
    public class PostbackAction : ITemplateAction
    {
        public string type { get; set; }
        /// <summary>
        /// Max: 20 characters
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// Max: 300 characters
        /// </summary>
        public string text { get; set; }

        public PostbackAction()
        {
            this.type = TemplateActionEnum.postback.ToString();
        }
    }

    /// <summary>
    /// When this action is tapped, the string in the text field is sent as a message from the user.
    /// </summary>
    public class MessageAction : ITemplateAction
    {
        public string type { get; set; }
        public string label { get; set; }
        /// <summary>
        /// Max: 300 characters
        /// </summary>
        public string text { get; set; }

        public MessageAction()
        {
            this.type = TemplateActionEnum.message.ToString();
        }
    }

    /// <summary>
    /// When this action is tapped, the URI specified in the uri field is opened.
    /// </summary>
    public class UriAction : ITemplateAction
    {
        public string type { get; set; }
        public string label { get; set; }
        /// <summary>
        /// URI opened when the action is performed (Max: 1000 characters)
        /// http, https, tel
        /// </summary>
        public string uri { get; set; }

        public UriAction()
        {
            this.type = TemplateActionEnum.uri.ToString();
        }
    }

    public interface ITemplateAction
    {
        string type { get; set; }
        string label { get; set; }
    }
}