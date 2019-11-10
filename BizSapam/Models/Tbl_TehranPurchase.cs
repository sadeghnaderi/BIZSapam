using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_TehranPurchase
    {
        [Required]
        public int Id { get; set; }

        public Tbl_User User { get; set; }
        [Required]
        [StringLength(10)]
        public string Price { get; set; }
        [StringLength(25)]
        [Required]
        public string DateTime { get; set; }

        public string Description { get; set; }
    }
}