using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class Category_ProductController : BaseController
    {
        WebBanHangOnlineDbContext db;
        // GET: Admin/Category_Product
        public ActionResult Index()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.ProductCategories.Where(x => x.Status == 1).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.ProductCategories.Where(x => x.Status == 1).ToList();
            return View(model);
        }
    }
}