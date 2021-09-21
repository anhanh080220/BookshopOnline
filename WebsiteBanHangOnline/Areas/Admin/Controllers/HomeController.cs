using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
  
    public class HomeController : BaseController
    {
        WebBanHangOnlineDbContext db;
        // GET: Admin/Home
        public ActionResult Index()
        {
                return View();
        }
        public ActionResult Logout()
        {
            Session.Remove(CommonConstants.ACCOUNT_SESSION);
          return  RedirectToAction("Index", "Login");
        }
    }
}