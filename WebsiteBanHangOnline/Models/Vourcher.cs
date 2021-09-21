namespace WebsiteBanHangOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vourcher")]
    public partial class Vourcher
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string VourcherCode { get; set; }

        public decimal? Price { get; set; }

        public decimal? ConditionPrice { get; set; }

        public DateTime? DateIssued { get; set; }

        public int? Status { get; set; }
    }
}
