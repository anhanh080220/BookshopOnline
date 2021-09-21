using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHangOnline.Models;

namespace WebsiteBanHangOnline.Controllers
{
    public class CartController : Controller
    {
        WebBanHangOnlineDbContext db;
        // GET: Cart
        
        public ActionResult Index()
        {
            db = new WebBanHangOnlineDbContext();
            var model = (List<CartItem>)Session["Cart"];
            if (model != null)
            {
                foreach (var item in model)
                {
                    if (item.Quantity == 0)
                    {
                        model.Remove(item);
                    }
                }
            }

            Session["Cart"] = model;
            return View(model);
        }

        public ActionResult AddCart(long ID, string Name, string PromotionPrice, string Image)
        {
            db = new WebBanHangOnlineDbContext();
            var lst = new List<CartItem>();
            
            decimal TotalCost = 0;
            if (Session["Cart"] == null)
            {
                CartItem cart = new CartItem();
                cart.Id = ID;
                cart.Name = Name;
                cart.ImageUrl = Image;
                cart.PromotionPrice = Convert.ToDecimal(PromotionPrice);
                cart.Quantity = 1;
                lst.Add(cart);
                Session["Cart"] = lst;
                Session["Total"] = 1;
                TotalCost = Convert.ToDecimal(PromotionPrice);
                Session["TotalCost"] = TotalCost.ToString("N0");
                return RedirectToAction("Index", "ProductCategory");
            }
            else
            {
                lst = (List<CartItem>)Session["Cart"];
                bool check = false;
                foreach (var item in lst)
                {
                    if (item.Id == ID)
                    {
                        item.Quantity += 1;
                        check = true;
                        break;
                    }
                }
                if (check == false)
                {
                    CartItem cart = new CartItem();
                    cart.Id = ID;
                    cart.Name = Name;
                    cart.ImageUrl = Image;
                    cart.PromotionPrice = Convert.ToDecimal(PromotionPrice);
                    cart.Quantity = 1;
                    lst.Add(cart);
                    Session["Cart"] = lst;
                }
                else
                {
                    Session["Cart"] = lst;
                }
                Session["Total"] = lst.Count;

                foreach (var item in lst)
                {
                    TotalCost = TotalCost + item.PromotionPrice * item.Quantity;
                }
                Session["TotalCost"] = TotalCost.ToString("N0");
                return RedirectToAction("Index", "ProductCategory");
            }
        }

        public ActionResult RemoveCart(long ID)
        {
            db = new WebBanHangOnlineDbContext();
            var model = (List<CartItem>)Session["Cart"];
            foreach (var item in model)
            {
                if (item.Id == ID)
                {
                    model.Remove(item);
                }
            }
            return RedirectToAction("Index","ProductCategory");
        }

        [HttpPost]
        public JsonResult UpdateProduct(string ID, string Quantity)
        {
            db = new WebBanHangOnlineDbContext();
            decimal TotalCost = 0;
            var model = (List<CartItem>)Session["Cart"];
            if (ID !=null && Quantity!=null)
            {
                foreach (var item in model)
                {
                    if (item.Id == Convert.ToInt32(ID))
                    {
                        item.Quantity = Convert.ToInt32(Quantity);
                        db.SaveChanges();
                    }
                    TotalCost = TotalCost + item.PromotionPrice * item.Quantity;
                }
                Session["TotalCost"] = TotalCost.ToString("N0");
                Session["Cart"] = model;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
            
        }
        public JsonResult GetAll(List<CartItem> _JSON)
        {
            Session["OrderProduct"] = _JSON;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckOut()
        {
            var lstOrder = (List<CartItem>)Session["OrderProduct"];
            if (lstOrder == null)
            {
                return RedirectToRoute("Shop Cart");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CheckOut(Order model)
        {
            db = new WebBanHangOnlineDbContext();
            var user = (User)Session[CommonConstants.ACCOUNT_SESSION];
            if (user == null)
            {
                return RedirectToRoute("Login Register");
            }
            var lstOrder = (List<CartItem>)Session["OrderProduct"];
            if (lstOrder == null)
            {
                return RedirectToRoute("Shop Cart");
            }
            else
            {
                model.CustomerID = user.ID;
                model.Status = 1;
                model.CreateDate = DateTime.Now;
                db.Orders.Add(model);
                db.SaveChanges();
                decimal total = 0;
                foreach (var item in lstOrder)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderID = model.ID;
                    if (item.PromotionPrice == 0)
                    {
                        orderDetail.Price = item.Price;
                        total = total + item.Price;
                    }
                    else
                    {
                        orderDetail.Price = item.PromotionPrice;
                        total = total + item.PromotionPrice;
                    }

                    orderDetail.ProductID = item.Id;
                    orderDetail.Quantity = item.Quantity;
                    db.OrderDetails.Add(orderDetail);
                    
                }
                db.SaveChanges();
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/assets/template/neworder.html"));
                content = content.Replace("{{CustomerName}}", model.ShipName);
                content = content.Replace("{{Phone}}", model.ShipMobile);
                content = content.Replace("{{Email}}", model.ShipEmail);
                content = content.Replace("{{Address}}", model.ShipAddress);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(model.ShipEmail, "Đơn hàng mới từ OnlineShop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
                return RedirectToRoute("Trang Chu");
            }
          
        }

    }
}