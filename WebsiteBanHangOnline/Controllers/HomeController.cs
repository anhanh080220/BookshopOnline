using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Controllers
{
   
    public class HomeController : Controller
    {
        WebBanHangOnlineDbContext db;
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
        
       
        public ActionResult MenuHeader()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Menu_Web.Where(x => x.Status == 1).OrderBy(x => x.OrderNumber).ToList();
            return PartialView(model);
        }

        public ActionResult Slider()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Slides.Where(x => x.Status == 1).OrderBy(x => x.DisplayOrder).ToList();
            return PartialView(model);
        }
        
        public ActionResult HomeSlideTap()
        {
            return PartialView();
        }

        public ActionResult StickyHeader()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Menu_Web.Where(x => x.Status == 1).OrderBy(x => x.OrderNumber).ToList();
            return PartialView(model);
        }

        public ActionResult NavigationMenu()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Menu_Web.Where(x => x.Status == 1).OrderBy(x => x.OrderNumber).ToList();
            return PartialView(model);
        }

        public ActionResult Footer()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Footers.Where(x => x.Status == 1).FirstOrDefault();
            return PartialView(model);
        }
    }
}