using System.Collections.Generic;
using lm = Linebot.Mn.Models.LineJsonModels;

namespace ZombieRunner.LineBot.WebHook.Cores.Bot
{
    public class MessageBuilder
    {

        public static lm.TemplateMessage GenCarouselTemplateMessage(string story)
        {
            var carouselMax = Cores.Strings.StoryCarouselMaxLen;
            var carouselNum = CountCarouselNumbers(carouselMax, story.Length);
            var carousels = new List<lm.CarouselColumObject>();
            for (int i = 0; i < carouselNum; i++)
            {
                carousels.Add(new lm.CarouselColumObject
                {
                    text = GetStorySlice(carouselMax, story, i),
                    actions = GenDefaultTemplateAction()
                });
            }
            var ct = new lm.TemplateMessage
            {
                altText = story,
                template = new lm.CarouselTemplateType
                {
                    columns = carousels.ToArray()
                }
            };
            return ct;
        }

        private static int CountCarouselNumbers(int carouselMax, int storyLength)
        {
            var carouselNum = storyLength % carouselMax == 0 ? storyLength / carouselMax : (storyLength / carouselMax) + 1;
            return carouselNum;
        }

        private static string GetStorySlice(int carouselMax, string story, int loopIdx)
        {
            var storyLen = story.Length;
            var sliceLen = carouselMax * (loopIdx + 1) <= storyLen ? carouselMax : storyLen - carouselMax * loopIdx - 1;
            var storySlice = $"{story.Substring(loopIdx * carouselMax, sliceLen)}";
            storySlice = loopIdx == 0 ? storySlice + "（...待續）" : "（...續上）" + storySlice;
            return storySlice;
        }

        private static lm.TemplateMessage GenButtonTemplateMessage(string story)
        {
            return new lm.TemplateMessage
            {
                altText = story,
                template = new lm.ButtonTemplateType
                {
                    title = "",
                    text = story,
                    thumbnailImageUrl = "https://zzombierunner.azurewebsites.net/content/images/bot/f640x532.jpg",
                    actions = new lm.ITemplateAction[] {
                        new lm.UriAction {
                            label = "倖存者日記",
                            uri = "https://zzombierunner.azurewebsites.net"
                        }
                    }
                }
            };
        }

        public static lm.TemplateMessage GenConfirmTemplateMessge(string story)
        {
            return new lm.TemplateMessage
            {
                altText = story,
                template = new lm.ConfirmTemplateType
                {
                    text = story,
                    actions = GenDefaultTemplateAction()
                }
            };
        }

        private static lm.ITemplateAction[] GenDefaultTemplateAction()
        {
            var ta = new lm.ITemplateAction[] {
                        new lm.UriAction {
                            label = "PIL",
                            uri = "http://f.pil.tw"
                        },
                        new lm.UriAction {
                            label = "倖存者日記",
                            uri = "https://zzombierunner.azurewebsites.net"
                        }
                    };
            return ta;
        }

    }
}