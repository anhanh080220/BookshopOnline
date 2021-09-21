using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        WebBanHangOnlineDbContext db;
        // GET: Admin/Footer
        public ActionResult Index()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Footers.Where(x => x.Status == 1).ToList();
            return View(model);
        }

        public ActionResult Edit(string id = "FooterClient")
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Footers.Find(id);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Footer model, FormCollection collection)
        {

            return View();
        }
    }
}