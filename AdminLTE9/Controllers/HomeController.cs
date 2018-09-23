using System.Web.Mvc;

namespace AdminLTE9.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
