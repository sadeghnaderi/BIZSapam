using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizSapam.Models;
using PagedList.Mvc;
using PagedList;

namespace BizSapam.ViewModels
{
    public class InvoiceIpagedList
    {
        public Tbl_Invoices Users { get; set; }
        public IPagedList<Tbl_Invoices> IpagedListInvoice { get; set; }
    }
}