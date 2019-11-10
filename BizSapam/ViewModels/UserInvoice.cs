using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizSapam.Models;

namespace BizSapam.ViewModels
{
    public class UserInvoice
    {
        public Tbl_User User { get; set; }
        public Tbl_Invoices Invoice { get; set; }
    }
}