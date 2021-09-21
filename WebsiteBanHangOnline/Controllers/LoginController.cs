using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        WebBanHangOnlineDbContext db;
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Account model)
        {
            db = new WebBanHangOnlineDbContext();
            var str = MD5.Encrypt(model.Password);
            var item = db.Accounts.Where(x => x.AccountName.Equals(model.Password)).FirstOrDefault();
            var user = db.Users.Where(x => x.AccountID == item.ID).FirstOrDefault();
            

            Session[CommonConstants.ACCOUNT_SESSION] = user;

            if (Session[CommonConstants.ACCOUNT_SESSION] == null)
            {
                ModelState.AddModelError("", "Tên tài khoản không tồn tại");
                return View();
            }
            else
            {
                if (item.Status == 0)
                {
                    ModelState.AddModelError("", "Tài khoản hết hiệu lực");
                    return View();
                }
                else
                {
                    if (item.Password.Equals(str) == false)
                    {
                        ModelState.AddModelError("", "Mật khẩu không chính xác");
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { id = item.ID });
                    }
                }
            }

        }

        [HttpPost]
        public JsonResult ReturnURL(string Email, string LastName, string FirstName, string GoogleID, string ProfileURL)
        {
            db = new WebBanHangOnlineDbContext();
            var item = db.Users.Where(x => x.Email.Equals(Email)).FirstOrDefault();
            Session[CommonConstants.ACCOUNT_SESSION] = item;

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "3968762243246477",
                client_secret = "f67c8fada8a8453589befbe434e20c8a",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "3968762243246477",
                client_secret = "f67c8fada8a8453589befbe434e20c8a",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code

            });

            var accessToken = result.access_token;
            Session["AccsessToken"] = accessToken;
            fb.AccessToken = accessToken;
            //string ans4 = fb.Get("me/friends", new { fields = new[] { "name, id" } }).ToString();
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string email = me.email;

            TempData["email"] = me.email;
            TempData["first_name"] = me.first_name;
            TempData["lastname"] = me.last_name;
            TempData["picture"] = me.picture.data.url;
            FormsAuthentication.SetAuthCookie(email, false);
            var item = new User();
            item.Email = TempData["email"].ToString();
            item.Name = TempData["first_name"].ToString() + " " + TempData["lastname"].ToString();
            Session[CommonConstants.ACCOUNT_SESSION] = item;
            return RedirectToAction("Index", "Home");
        }
    }
}