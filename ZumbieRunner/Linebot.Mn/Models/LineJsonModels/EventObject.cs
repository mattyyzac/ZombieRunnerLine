using Linebot.Mn.Models.LineJsonModels.Enums;
using System.Collections.Generic;

namespace Linebot.Mn.Models.LineJsonModels
{
    /// <summary>
    /// line web hook json data
    /// </summary>
    public class EventResult
    {
        public IEnumerable<EventObject> events { get; set; }

        /// <summary>
        /// custom field, describing is parsing correctly ?
        /// </summary>
        public bool? IsParseOk { get; set; }
    }
    public class EventObject
    {
        public string replyToken { get; set; }
        public EventTypeEnum type { get; set; }
        public long timestamp { get; set; }
        public UserSource source { get; set; }
        public EventTextMessage message { get; set; }
    }
    public class UserSource : ISource
    {
        public string type { get; set; }
        public string userId { get; set; }
        
        public UserSource()
        {
            this.type = SourceTypeEnum.user.ToString();
        }
    }
    public class GroupSource : ISource
    {
        public string type { get; set; }
        public string userId { get; set; }
        public string groupId { get; set; }

        public GroupSource()
        {
            this.type = SourceTypeEnum.group.ToString();
        }
    }

    public class RoomSource : ISource
    {
        public string type { get; set; }
        public string userId { get; set; }
        public string roomId { get; set; }

        public RoomSource()
        {
            this.type = SourceTypeEnum.room.ToString();
        }
    }

    public interface ISource
    {
        string type { get; set; }
        string userId { get; set; }
    }

    public enum SourceTypeEnum
    {
        user,
        group,
        room
    }
    public class EventTextMessage : IEnventMessage
    {
        public string @type { get; set; }
        public string id { get; set; }
        public string text { get; set; }

        public EventTextMessage()
        {
            this.@type = MessageTypeEnum.text.ToString();
        }
    }
    public interface IEnventMessage
    {
        string @type { get; set; }
        string id { get; set; }
    }
}