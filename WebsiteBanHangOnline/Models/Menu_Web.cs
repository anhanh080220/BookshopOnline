namespace WebsiteBanHangOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Menu_Web
    {
        public int ID { get; set; }

        public int? ParentID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        public long? CategoryID { get; set; }

        public long? NewsID { get; set; }

        [StringLength(250)]
        public string Page { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public int? Level { get; set; }

        public int? OrderNumber { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(250)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        public int? Status { get; set; }
    }
}
