using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        WebBanHangOnlineDbContext db;

        // GET: Admin/User
        public ActionResult Index()
        {
            //Người đang đăng nhập
            var Now = (User)Session[CommonConstants.ACCOUNT_SESSION];
            //Danh sách tài khoản
            db = new WebBanHangOnlineDbContext();
            var model = (from a in db.Accounts
                         join b in db.Users
                         on a.ID equals b.AccountID
                         select new {
                             objectAcc = a,
                             objectUser =b
                         }).ToList();
            List<tblAccountUser> lstAccUser = new List<tblAccountUser>();
            foreach (var item in model)
            {
                tblAccountUser accUser = new tblAccountUser();
                accUser.ID = item.objectUser.ID;
                accUser.AccountName = item.objectAcc.AccountName;
                accUser.Account_ID = item.objectAcc.ID;
                accUser.Address = item.objectUser.Address;
                accUser.Birthday = item.objectUser.Birthday;
                accUser.CreatedDate = item.objectAcc.CreatedDate;
                accUser.CreatedUser = item.objectAcc.CreatedUser;
                accUser.DateIssued = item.objectAcc.DateIssued;
                accUser.Phone = item.objectUser.Phone;
                accUser.Description = "";
                accUser.Image = item.objectUser.Image;
                accUser.Password = item.objectAcc.Password;
                accUser.Sex = item.objectUser.Sex;
                accUser.Status = 1;
                accUser.UserName = item.objectUser.Name;
                accUser.Email = item.objectUser.Email;
                accUser.ModifiedDate = item.objectAcc.ModifiedDate;
                accUser.ModifiedUser = item.objectAcc.ModifiedUser;
                lstAccUser.Add(accUser);
            }
            return View(lstAccUser);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblAccountUser model, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
            db = new WebBanHangOnlineDbContext();
            var us = (User)Session[CommonConstants.ACCOUNT_SESSION];
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
                string check = collection["groupSex"].ToString();
                var acc = db.Accounts.Where(x => x.Status == 1).ToList();
                foreach (var itemAcc in acc)
                {
                    if (itemAcc.AccountName.Equals(model.AccountName))
                    {
                        ModelState.AddModelError("", "Tên tài khoản đã tồn tại");
                        return View();
                    }
                }
                //tạo account mới
                Account newAcc = new Account();
                newAcc.AccountName = model.AccountName;
                newAcc.Password = MD5.Encrypt(model.Password);
                newAcc.CreatedDate = DateTime.Now;
                newAcc.CreatedUser = us.Name;
                newAcc.DateIssued = model.DateIssued;
                newAcc.ModifiedDate = null;
                newAcc.ModifiedUser = null;
                newAcc.Status = 1;
                db.Accounts.Add(newAcc);
                db.SaveChanges();
                //tạo user mới
                User newUser = new User();
                newUser.Address = model.Address;
                newUser.Name = model.UserName;
                newUser.CreateDate = DateTime.Now;
                newUser.CreateBy = us.Name;
                newUser.Birthday = model.Birthday;
                newUser.Email = model.Email;
                newUser.Phone = model.Phone;
                newUser.AccountID = newAcc.ID;
                newUser.ModifiedDate = null;
                newUser.ModifiedBy = null;
                int iContent = fileLocation.IndexOf("Data");
                if (iContent > 0)
                {
                    newUser.Image = @"\" + fileLocation.Substring(iContent, fileLocation.Length - iContent);
                }
                if (check.Equals("1"))
                {
                        newUser.Sex = 1;
                }
                else
                {
                        newUser.Sex = 0;
                }
                newUser.Status = 1;
                db.Users.Add(newUser);
                db.SaveChanges();
                SetAlert("Thêm thành công", "success");
                return RedirectToAction("Index", "User");
            }
            else
            {
                SetAlert("Thêm thất bại", "error");
                return RedirectToAction("Index", "User");
            }

          
        }
        public ActionResult Edit(int ID)
        {
            db = new WebBanHangOnlineDbContext();
            var user = db.Users.Find(ID);
            var acc = db.Accounts.Find(user.AccountID);
            tblAccountUser accUser = new tblAccountUser();
            accUser.ID = user.ID;
            accUser.AccountName = acc.AccountName;
            accUser.UserName = user.Name;
            accUser.Account_ID = acc.ID;
            accUser.Address = user.Address;
            accUser.Birthday = user.Birthday;
            accUser.CreatedDate = acc.CreatedDate;
            accUser.CreatedUser = acc.CreatedUser;
            accUser.DateIssued = acc.DateIssued;
            accUser.Description = "";
            accUser.Email = user.Email;
            accUser.Image = user.Image;
            accUser.ModifiedDate = acc.ModifiedDate;
            accUser.ModifiedUser = acc.ModifiedUser;
            accUser.Password = MD5.Decrypt(acc.Password) ;
            accUser.Phone = user.Phone;
            accUser.Sex = user.Sex;
            accUser.Status = acc.Status;
            return View(accUser);
        }
        [HttpPost]
        public ActionResult Edit(tblAccountUser model , FormCollection collection)
        {

            if (ModelState.IsValid)
            {
                db = new WebBanHangOnlineDbContext();
                var user = db.Users.Find(model.ID);
                var acc = db.Accounts.Find(user.AccountID);
                var us = (User)Session[CommonConstants.ACCOUNT_SESSION];
                string check = collection["groupSex"].ToString();
                string fileLocation = "";
                if (Request.Files["filePost"].ContentLength <= 0) { model.Image = user.Image; }
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
                //update Account
                acc.AccountName = model.AccountName;
                acc.CreatedDate = model.CreatedDate;
                acc.CreatedUser = model.CreatedUser;
                acc.DateIssued = model.DateIssued;
                acc.ModifiedDate = DateTime.Now;
                acc.ModifiedUser = us.Name;
                acc.Password = MD5.Encrypt(model.Password);
                acc.Status = 1;
                db.SaveChanges();
                //update User
                user.AccountID = acc.ID;
                user.Address = model.Address;
                user.Birthday = model.Birthday;
                user.CreateBy = model.CreatedUser;
                user.CreateDate = model.CreatedDate;
                user.Email = model.Email;
                user.ModifiedBy = us.Name;
                user.Phone = model.Phone;
                user.Status = 1;
                user.ModifiedBy = us.Name;
                user.ModifiedDate = DateTime.Now;
                if (check.Equals("1"))
                {
                    user.Sex = 1;
                }
                else
                {
                    user.Sex = 0;
                }
                int iContent = fileLocation.IndexOf("Data");
                if (iContent > 0)
                {
                    user.Image = @"\" + fileLocation.Substring(iContent, fileLocation.Length - iContent);
                }
                db.SaveChanges();
                SetAlert("Cập nhật thành công", "success");
                return RedirectToAction("Index","User");
            }
            return View(model);
        }
    }
}