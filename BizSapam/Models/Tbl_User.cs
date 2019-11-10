using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BizSapam.Models
{
    public class Tbl_User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(30)]
        public string BIZCode { get; set; }
        [StringLength(25)]
        public string Username { get; set; }
        [StringLength(25)]
        public string Password { get; set; }
        [ForeignKey("Tbl_AccessLevels")]
        public byte AccessLevelID { get; set; }
        public virtual Tbl_AccessLevels Tbl_AccessLevels { get; set; }
        public string Picture { get; set; }
        public byte Active { get; set; }
    }
}