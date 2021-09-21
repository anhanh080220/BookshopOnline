namespace WebsiteBanHangOnline.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Role
    {
        public long ID { get; set; }

        public long? UserID { get; set; }

        [StringLength(50)]
        public string RoleID { get; set; }
    }
}
