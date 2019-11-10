using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_LogTypes
    {
        [Required]
        public byte Id { get; set; }

        public string Type { get; set; }
    }
}