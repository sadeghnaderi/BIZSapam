using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_Invoices
    {
        [Required]
        public int Id { get; set; }
        [ForeignKey("Tbl_User")]
        public int UserID { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }

        [ForeignKey("SellerId2")]
        public int SellerID { get; set; }
        public virtual Tbl_User SellerId2 { get; set; }
        [Required]
        [StringLength(25)]
        public string DateTime { get; set; }
        [ForeignKey("Tbl_InvoiceStates")]
        public byte InvoiceStatesID { get; set; }
        public virtual Tbl_InvoiceStates Tbl_InvoiceStates { get; set; }
        public string Description { get; set; }

    }
}