using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cis237inclass6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //Add my own method that should be reachbe b
        //the url: /Home/Foo
        [Authorize]
        public ActionResult Foo()
        {
            ViewBag.MyMagicalFooProperty = "All the Fooos";
            return View();
        }
    }
}