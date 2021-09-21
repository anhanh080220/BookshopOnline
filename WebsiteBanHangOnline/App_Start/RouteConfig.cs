using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteBanHangOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Product Category",
              url: "san-pham/{metatitle}-{cateId}",
              defaults: new { controller = "ProductCategory", action = "Category", id = UrlParameter.Optional },
              new[] { "WebsiteBanHangOnline.Controllers" }
          );

            routes.MapRoute(
              name: "List All Product",
              url: "san-pham",
              defaults: new { controller = "ProductCategory", action = "Index", id = UrlParameter.Optional },
              new[] { "WebsiteBanHangOnline.Controllers" }
          );
            routes.MapRoute(
            name: "Detail Product",
            url: "chi-tiet/{metatitle}-{Id}",
            defaults: new { controller = "ProductCategory", action = "Detail", id = UrlParameter.Optional },
            new[] { "WebsiteBanHangOnline.Controllers" }
          );
            routes.MapRoute(
              name: "Shop Cart",
              url: "shop-cart",
              defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
              new[] { "WebsiteBanHangOnline.Controllers" }
          );
            routes.MapRoute(
              name: "Login Register",
              url: "login-register",
              defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
              new[] { "WebsiteBanHangOnline.Controllers" }
          );
                routes.MapRoute(
               name: "Checkout",
               url: "checkout",
               defaults: new { controller = "Cart", action = "CheckOut", id = UrlParameter.Optional },
               new[] { "WebsiteBanHangOnline.Controllers" }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "WebsiteBanHangOnline.Controllers" }
            );


        }
    }
}
