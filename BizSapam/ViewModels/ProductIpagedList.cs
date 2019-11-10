using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizSapam.Models;
using PagedList.Mvc;
using PagedList;

namespace BizSapam.ViewModels
{
    public class ProductIpagedList
    {
        public Tbl_Products Products { get; set; }
        public IPagedList<Tbl_Products> IpagedListProduct { get; set; }
    }
}