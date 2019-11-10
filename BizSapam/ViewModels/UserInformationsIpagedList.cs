using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizSapam.Models;
using PagedList.Mvc;
using PagedList;

namespace BizSapam.ViewModels
{
    public class UserInformationsIpagedList
    {
        public UserShopingInfo UserShopingInfo { get; set; }
        public IPagedList<UserShopingInfo> IpagedListUserInfo { get; set; }
    }
}