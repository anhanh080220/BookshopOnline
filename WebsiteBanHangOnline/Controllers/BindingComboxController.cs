using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Controllers
{
    public class BindingComboxController : Controller
    {
        WebBanHangOnlineDbContext db;
        // GET: BindingCombox
        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult GetComBoDanhMuc()
        {
            db = new WebBanHangOnlineDbContext();
         
            var lstDanhMuc = db.ProductCategories.Where(x=>x.ID == 7 || x.ID ==33).ToList();
            var selectList = new List<SelectListItem>();
            foreach (var item in lstDanhMuc)
            {
                selectList.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                });
            }
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetComBoSanPham(string idDanhMuc)
        {
            db = new WebBanHangOnlineDbContext();
            long a = Convert.ToInt64(idDanhMuc);
            var lstDanhMuc = db.Products.Where(x => x.CategoryID == a).ToList();
            var selectList = new List<SelectListItem>();
            foreach (var item in lstDanhMuc)
            {
                selectList.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                });
            }
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
    }
}