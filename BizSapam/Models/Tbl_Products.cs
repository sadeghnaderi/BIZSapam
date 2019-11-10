using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_Products
    {
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        public string Barcode { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }
        [StringLength(10)]
        public string Price { get; set; }
        public short Qty { get; set; }
        public string Description { get; set; }
    }
}