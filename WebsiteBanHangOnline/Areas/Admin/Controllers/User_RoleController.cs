using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class User_RoleController : BaseController
    {
        WebBanHangOnlineDbContext db;
        // GET: Admin/User_Role
        public ActionResult Index()
        {
            setTable();
            return View();
        }
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Create_Role(Role model)
        {
            db = new WebBanHangOnlineDbContext();
            var user = (User)Session[CommonConstants.ACCOUNT_SESSION];
            var role = db.Roles.Where(x => x.Status == 1).ToList();
            foreach (var item in role)
            {
                if (item.RoleID.Equals(model.RoleID))
                {
                    ModelState.AddModelError("", "Mã quyền đã tồn tại");
                    return Json("FailID", JsonRequestBehavior.AllowGet);
                }
            }
            foreach (var item in role)
            {
                if (item.RoleName.Equals(model.RoleName))
                {
                    ModelState.AddModelError("", "Tên quyền đã tồn tại");
                    return Json("FailName", JsonRequestBehavior.AllowGet);
                }
            }
            model.CreateDate = DateTime.Now;
            model.CreateBy = user.Name;
            model.Status = 1;
            if (ModelState.IsValid)
            {
                db.Roles.Add(model);
                var result = db.SaveChanges();
                if (result == 1)
                {
                    SetAlert("Thêm thành công", "success");
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

            public JsonResult SaveData(string ID, String Choice)
        {
            //Thực hiện cập nhật dữ liệu
            var strID = ID.Replace("chk_", "").Replace(" ", "").Trim();
            var arrID = strID.Split('_');
            var RoleID = arrID[0].ToString();
            var UserID = arrID[1].ToString();
            db = new WebBanHangOnlineDbContext();
            if (Choice.ToUpper() == "TRUE")
            {
                //Thực hiện ghi dữ liệu
                var item = new User_Role();
                item.ID = -1;
                item.UserID = Convert.ToInt32(UserID);
                item.RoleID = RoleID;

                db.User_Role.Add(item);
                var result = db.SaveChanges();
                if (result > 0)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
               
            }
            else
            {
                //Xóa dữ liệu
                var uID = Convert.ToInt32(UserID);
                var item = db.User_Role.Where(x => x.RoleID == RoleID && x.UserID == uID).SingleOrDefault();
                var Result = db.User_Role.Remove(item);
                db.SaveChanges();
                if (Result != null)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
             
            }
            return Json("Fail", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            setTable();

            //Xóa dữ liệu
            for (int i = 1; i < collection.AllKeys.Count(); i++)
            {
                if (collection.AllKeys[i].Contains('_') == true)
                {
                    var arr = collection.AllKeys[i].ToString().Split('_');
                    string MA_USER = arr[2].ToString().Trim();
                }
            }
            //Ghi dữ liệu
            string resInsert = "";
            for (int i = 1; i < collection.AllKeys.Count(); i++)
            {
                if (collection.AllKeys[i].Contains("_") == true)
                {
                    var arr = collection.AllKeys.ToString().Split('_');
                    string MA_QUYEN = arr[1].ToString().Trim();
                    string MA_USER = arr[2].ToString().Trim();
                }
            }

            if (resInsert == "OK")
            {
                //SetAlert("Success", "Thêm mới dữ liệu thành công");
                return RedirectToAction("Index", "Role_User");
            }
            else
            {
                //SetAlert("Error", "Lỗi trong quá trình thêm dữ liệu:" + resInsert);
                return View();
            }
        }

        public void setTable()
        {
            db = new WebBanHangOnlineDbContext();
            var role = db.Roles.Where(x => x.Status == 1).ToList();
            Session[CommonConstants.ROLE] = role;

            var user = db.Users.Where(x => x.Status == 1).ToList();
            Session[CommonConstants.USER] = user;

            var role_user = (from a in db.Users
                             join b in db.User_Role
                             on a.ID equals b.UserID
                             where a.Status == 1
                             select new
                             {
                                 ID = a.ID,
                                 UserName = a.Name,
                                 RoleID = b.RoleID,
                             }).ToList();

            List<User_Role> lstUserRole = new List<User_Role>();
            foreach (var item in role_user)
            {
                User_Role tbl = new User_Role();
                tbl.ID = item.ID;
                tbl.UserID = item.ID;
                tbl.RoleID = item.RoleID;
                lstUserRole.Add(tbl);
            }
            Session[CommonConstants.ROLE_USER] = lstUserRole;

        }
    }
}