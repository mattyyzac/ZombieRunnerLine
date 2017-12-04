using Linebot.Mn.Models.LineJsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Linebot.Mn
{
    /// <summary>
    /// 訊息傳送引擎
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// 訊息
        /// </summary>
        public List<IMessage> DestinationMessages { get; set; }
        /// <summary>
        /// 傳送目的
        /// </summary>
        public IEnumerable<string> DestinationUserIds { get; set; }

        public Engine(IEnumerable<string> _destUserIds)
        {
            this.DestinationMessages = new List<IMessage>();
            this.DestinationUserIds = _destUserIds;
        }

        #region simple format messages

        public async Task<HttpStatusCode> SendTextMessage(IEnumerable<string> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => new TextMessage
            {
                text = o
            }));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }

        public async Task<HttpStatusCode> SendImageMessage(IEnumerable<ImageMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => new ImageMessage
            {
                originalContentUrl = o.originalContentUrl,
                previewImageUrl = o.previewImageUrl
            }));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);

        }

        public async Task<HttpStatusCode> SendAudioMessage(IEnumerable<AudioMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => new AudioMessage {
                duration = o.duration,
                originalContentUrl = o.originalContentUrl
            }));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }

        public async Task<HttpStatusCode> SendVideoMessage(IEnumerable<VideoMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => new VideoMessage
            {
                previewImageUrl = o.previewImageUrl,
                originalContentUrl = o.originalContentUrl
            }));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }

        public async Task<HttpStatusCode> SendLocationMessage(IEnumerable<LocationMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => new LocationMessage
            {
                address = o.address,
                title = o.title,
                latitude = o.latitude,
                longitude = o.longitude
            }));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }

        public async Task<HttpStatusCode> SendStickerMessage(IEnumerable<StickerMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => new StickerMessage
            {
                packageId = o.packageId,
                stickerId = o.stickerId
            }));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }
        #endregion
        #region templated messages

        public async Task<HttpStatusCode> SendTemplateMessage(IEnumerable<TemplateMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => o));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }
        #endregion
        #region multiple messages, allow different message type

        public async Task<HttpStatusCode> SendMultipleMessages(IEnumerable<IMessage> messages, string token = "")
        {
            this.DestinationMessages.AddRange(messages.Select(o => o));
            if (string.IsNullOrEmpty(token))
            {
                return await BaseSendMessage(this.DestinationMessages.ToArray(), this.DestinationUserIds);
            }
            return await BaseSender(token, this.DestinationMessages.ToArray(), this.DestinationUserIds);
        }
        #endregion

        #region System.Net.Http sender helper

        /// <summary>
        /// Bases the send message.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="destUserIds">The dest user ids.</param>
        private static async Task<HttpStatusCode> BaseSender(string accessToken, IEnumerable<IMessage> messages, IEnumerable<string> destUserIds)
        {
            const int MESSAGE_MAX = 5;
            const int RECEIVER_MAX = 150;

            if (!messages.Any() || !destUserIds.Any())
            {
                throw new ArgumentException();
            }
            if (messages.Count() > MESSAGE_MAX || destUserIds.Count() > RECEIVER_MAX)
            {
                messages = messages.Take(MESSAGE_MAX);
                destUserIds = destUserIds.Take(RECEIVER_MAX);
            }
            try
            {
                var client = Cores.Common.SetHttpClientHeader(accessToken);
                var apiUrl = Cores.Strings.PushMessageApi;
                var post = await client.PostAsJsonAsync(apiUrl, new
                {
                    to = destUserIds,
                    messages = messages
                });
                return post.StatusCode;
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends message by line bot multicast api, line access token will come from dll-app.config
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="destinations">The destinations.</param>
        private static async Task<HttpStatusCode> BaseSendMessage(IEnumerable<IMessage> messages, IEnumerable<string> destUserIds)
        {
            var token = Cores.Strings.LineBotAccessToken;
            return await BaseSender(token, messages.Take(5), destUserIds);
        }
        #endregion
    }
}