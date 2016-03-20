using System.Web.Mvc;

namespace Lymer.UserUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RecentTweets()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult RecentBlogArticles()
        {
            return PartialView();
        }
    }
}
