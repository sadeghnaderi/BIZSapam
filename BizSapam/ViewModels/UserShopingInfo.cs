using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSapam.ViewModels
{
    public class UserShopingInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BIZCode { get; set; }
        public Int64 TotalPurchase { get; set; }
        public string TotalBIZCredit { get; set; }
        public string TehranPurchase { get; set; }
        public string MaxPurchaseAllowed { get; set; }

    }
}