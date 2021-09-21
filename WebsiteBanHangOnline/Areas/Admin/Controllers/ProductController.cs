using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;
using System.Text.RegularExpressions;
using System.Text;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        WebBanHangOnlineDbContext db;
        // GET: Admin/Product
        public ActionResult Index()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Products.Where(x => x.Status == 1).ToList();
            ViewBag.category = db.ProductCategories.Where(x => x.Status == 1).ToList();
            return View(model);
        }
        // Create: Thêm mới sản phẩm

        public ActionResult Create()
        {
            db = new WebBanHangOnlineDbContext();
            ViewBag.category = db.ProductCategories.Where(x => x.Status == 1).ToList();


            return View();
        }

        [HttpPost]
        public ActionResult Create(Product model , FormCollection collection)
        {
            db = new WebBanHangOnlineDbContext();
            var user = (User)Session[CommonConstants.ACCOUNT_SESSION];
            string fileLocation = "";
            if (Request.Files["filePost"].ContentLength <= 0) { model.Image = ""; }
            model.Image = "/Areas/Admin/Content/assets/images/small/img-1.jpg";


            if (Request.Files["filePost"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["filePost"].FileName);
                fileLocation = Server.MapPath("~/Data/files/") + Request.Files["filePost"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["filePost"].SaveAs(fileLocation);
            }
            int iContent = fileLocation.IndexOf("Data");
            if (iContent > 0)
            {
                model.Image = @"\" + fileLocation.Substring(iContent, fileLocation.Length - iContent);
            }
            model.CreateBy = user.Name;

            model.CreateDate = DateTime.Now;
            model.MetaTitle = bodau(model.Name).Trim().ToLower().Replace(" ", "-");
            model.Status = 1;
            model.CategoryID = Convert.ToInt64(collection["cboTheLoai"].ToString());
            if (ModelState.IsValid)
            {
                db.Products.Add(model);
                db.SaveChanges();
                SetAlert("Thêm thành công", "success");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                SetAlert("Thêm thất bại", "error");
                return View();
            }
           
        }
        // Create: Chỉnh sửa thông tin sản phẩm
        public ActionResult Edit(long id)
        {
            db = new WebBanHangOnlineDbContext();
            ViewBag.category = db.ProductCategories.Where(x => x.Status == 1).ToList();
            var model = db.Products.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Product model , FormCollection collection)
        {
            db = new WebBanHangOnlineDbContext();
            ViewBag.category = db.ProductCategories.Where(x => x.Status == 1).ToList();
            var user = (User)Session[CommonConstants.ACCOUNT_SESSION];
            var item = db.Products.Where(x => x.ID == model.ID).FirstOrDefault();
            string fileLocation = "";
            if (Request.Files["filePost"].ContentLength <= 0) { item.Image = model.Image; }

            if (Request.Files["filePost"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["filePost"].FileName);
                fileLocation = Server.MapPath("~/Data/files/") + Request.Files["filePost"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["filePost"].SaveAs(fileLocation);
            }
           
           
            item.Name = model.Name;
            item.CategoryID = Convert.ToInt64(collection["cboTheLoai"].ToString());
            item.Code = model.Code;
            item.Description = item.Description;
            int iContent = fileLocation.IndexOf("Data");
            if (iContent > 0)
            {
                item.Image = @"\" + fileLocation.Substring(iContent, fileLocation.Length - iContent);
            }
            item.MetaTitle = bodau(model.Name).Trim().ToLower().Replace(" ", "-");
            item.ModifiedBy = user.Name;
            item.ModifiedDate = DateTime.Now;
            item.Price = model.Price;
            item.PromotionPrice = model.PromotionPrice;
            item.Status = 1;
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                SetAlert("Cập nhật thông tin thành công", "success");
                return RedirectToAction("Index","Product");
                
            }
            else
            {
                SetAlert("Cập nhật thông tin thất bại", "error");
                return View(model);
                
            }
           
        }
        public string bodau(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        }
    }
}