using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebsiteBanHangOnline.Models
{
    [Table("tblAccountUser")]
    public partial class tblAccountUser
    {
        public long ID { get; set; }

        public long Account_ID { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(500)]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateIssued { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public int? Sex { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        [StringLength(350)]
        public string Address { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedUser { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedUser { get; set; }

        [DisplayName("Upload Anh")]
        public string Image { get; set; }

    }
}