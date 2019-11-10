using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_InvoiceItems
    {
        [Required]
        public int Id { get; set; }

        [ForeignKey("Tbl_Invoices")]
        public int InvoiceId { get; set; }
        public virtual Tbl_Invoices Tbl_Invoices { get; set; }

        [ForeignKey("Tbl_Products")]
        public int ProductId { get; set; }
        public virtual Tbl_Products Tbl_Products { get; set; }

        [Required]
        [StringLength(10)]
        public string Price { get; set; }
        [Required]
        public short Qty { get; set; }

        public string Description { get; set; }
    }
}