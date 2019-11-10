using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_UserLogs
    {
        public int Id { get; set; }
        [Required]
        public Tbl_User User { get; set; }
        public Tbl_LogTypes LogTypes { get; set; }
        [Required]
        [StringLength(25)]
        public string DateTime { get; set; }

    }
}