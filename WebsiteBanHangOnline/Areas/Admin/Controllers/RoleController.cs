using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        WebBanHangOnlineDbContext db;

        // GET: Admin/Role
        public ActionResult Index()
        {
            db = new WebBanHangOnlineDbContext();
            var model = db.Roles.Where(x => x.Status == 1).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Role model , FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                db = new WebBanHangOnlineDbContext();
                string check = collection["groupStatus"].ToString();
                model.CreateDate = DateTime.Now;
                if (check.Equals("1"))
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                db.Roles.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Role");
            }
            return View();
        }

    
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                db = new WebBanHangOnlineDbContext();
                var model = db.Roles.Find(id);
                model.Status = 0;
                db.SaveChanges();
                return RedirectToAction("Index", "Role");
               
            }
            else
            {
                return View();
            }
           
        }
    }
}