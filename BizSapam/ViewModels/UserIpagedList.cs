using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizSapam.Models;
using PagedList.Mvc;
using PagedList;

namespace BizSapam.ViewModels
{
    public class UserIpagedList
    {
        public Tbl_User Users { get; set; }
        public List<Tbl_AccessLevels> AccessLevels { get; set; }
        public IPagedList<Tbl_User> IpagedListUser { get; set; }
    }
}