namespace WebsiteBanHangOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int ID { get; set; }

        [Column(TypeName = "ntext")]
        public string Contet { get; set; }

        public int? Status { get; set; }
    }
}
