using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHangOnline.Models
{
    public partial class CartItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}