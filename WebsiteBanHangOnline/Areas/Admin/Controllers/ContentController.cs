using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        WebBanHangOnlineDbContext db;
        // GET: Admin/Content
        public ActionResult Index()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Contents.Where(x => x.Status == 1).ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            SetViewbag();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]

        public ActionResult Create(Content model, FormCollection collection)
        {
            db = new WebBanHangOnlineDbContext();
            model.Status = 1;
            model.CategoryID = long.Parse(collection["cboCategory"].ToString());
            db.Contents.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index","Content");
        }
        public void SetViewbag()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Categories.Where(x => x.Status == 1).ToList();
            ViewBag.Category = model;
        }
    }
}