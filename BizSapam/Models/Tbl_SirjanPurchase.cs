using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_SirjanPurchase
    {
        [Required]
        public int Id { get; set; }
        [ForeignKey("Tbl_User")]
        public int UserId { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }
        [ForeignKey("Tbl_PurchaseTypes")]
        public byte PurchaseId { get; set; }
        public virtual Tbl_PurchaseTypes Tbl_PurchaseTypes { get; set; }
        [Required]
        [StringLength(10)]
        public string Price { get; set; }
        [Required]
        [StringLength(25)]
        public string DateTime { get; set; }
        public DateTime MiladiDate { get; set; }
        public string Description { get; set; }

    }
}