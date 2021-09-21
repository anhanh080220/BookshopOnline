using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Controllers
{
    public class ProductCategoryController : Controller
    {
        WebBanHangOnlineDbContext db;
        // GET: ProductCategory
        public ActionResult Index(long ID = 0, int page = 1, int pageSize = 12)
        {
            db = new WebBanHangOnlineDbContext();
            //ViewBag.searchString = searchString;
            double totalRecord = db.Products.Where(x => x.Status == 1).Count();
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            double totalPage = 0;
            totalPage = (double)Math.Ceiling(((decimal)(totalRecord / pageSize)));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            var model = db.Products.Where(x => x.Status == 1).OrderByDescending(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    var model = db.tblSaches.Where(x => x.TRANG_THAI == 1 ).OrderByDescending(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //    return View(model);
            //}
            //else
            //{
            //    var model = db.tblSaches.Where(x => x.TRANG_THAI == 1 && x.MA_THELOAI.Equals(theLoai)).OrderByDescending(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //    return View(model);
            //}
            return View(model);
        }
        //Danh sách sản phẩm
        public ActionResult CategoryProduct()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.ProductCategories.Where(x => x.Status == 1).OrderBy(x => x.DisplayOrder).ToList();
            return PartialView(model);
        }
        //Danh sách sản phẩm theo thể loại
        public ActionResult Category(long cateId ,int page = 1, int pageSize = 3)
        {
            db = new WebBanHangOnlineDbContext();
            //ViewBag.searchString = searchString;
            double totalRecord = db.Products.Where(x => x.Status == 1 && x.CategoryID == cateId).Count();
            ViewBag.Category = db.ProductCategories.Find(cateId);
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            double totalPage = 0;
            totalPage = (double)Math.Ceiling(((decimal)(totalRecord / pageSize)));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            var model = db.Products.Where(x => x.Status == 1 && x.CategoryID == cateId).OrderByDescending(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(model);
        }
        //Chi tiết sản phẩm
        public ActionResult Detail(long Id)
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Products.Find(Id);
            ViewBag.category = db.ProductCategories.Where(x => x.ID == model.CategoryID).FirstOrDefault();
            ViewBag.releated = db.Products.Where(x => x.ID != Id && x.CategoryID == model.CategoryID).Take(4).ToList();
            ViewBag.sale = 100 - Convert.ToInt32((model.PromotionPrice / model.Price) * 100);
            return View(model);
        }
    }
}