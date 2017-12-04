using System.Threading.Tasks;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace ZombieRunner.LineBot.WebHook.Controllers
{
    public class StoryController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> Go()
        {
            var stories = await new Cores.StoryMaker().Go();
            if (stories != null)
            {
                return Json(stories.Stories);
            }
            return Json(new { });
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [HttpGet]
        public async Task<IHttpActionResult> Demo()
        {
            var stories = await new Cores.StoryMaker().Demo();
            if (stories != null)
            {
                return Json(stories.Stories);
            }
            return Json(new { });
        }
    }
}
