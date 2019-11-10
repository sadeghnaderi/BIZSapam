using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using BizSapam.Models;


namespace BizSapam.Controllers
{

    public class SellerPanelController : Controller
    {
        private MyDBContext _context;
        public SellerPanelController()
        {
            _context = new MyDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: SellerPanel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            if (Session["UserId"] != null)
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                var DbUser = _context.Tbl_User.SingleOrDefault(u => u.Id == UserId);
                Tbl_User User = DbUser;
                return View(User);
            }
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public JsonResult GetNames(string Prefix)
        {

            //Note : you can bind same list from database  
            var ObjList = _context.Tbl_User.ToList();
            //Searching records from list using LINQ query  
            var UserList = (from N in ObjList
                            where (N.Name.Contains(Prefix) && N.AccessLevelID == 3)
                            select new { N.Name, N.BIZCode, N.Id });
            return Json(UserList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddInvoice(int id)
        {
            if (Session["UserId"] != null)
            {
                int LastInvoiceId = (_context.Database.SqlQuery<int>(@"select top 1 Id from Tbl_Invoices order by Id desc").FirstOrDefault<int>()) + 1;
                ViewBag.InvoiceId = LastInvoiceId;

                PersianDateTime now = PersianDateTime.Now;
                var DateTime = now.ToString("dddd d MMMM yyyy ساعت hh:mm:ss tt");
                ViewBag.DateTime = DateTime;

                var DbUser = _context.Tbl_User.SingleOrDefault(u => u.BIZCode == id.ToString());
                ViewBag.Name = DbUser.Name;

                return View();
            }
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public JsonResult AddInvoice(string Prefix)
        {
            //Note : you can bind same list from database  
            var ObjList = _context.Tbl_Products.ToList();
            //Searching records from list using LINQ query  
            var ProductList = (from N in ObjList
                               where N.ProductName.Contains(Prefix)
                               select new { N.ProductName, N.Price, N.Id });
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetProductInfo(string ProductName)
        {
            var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.ProductName == ProductName);

            return Json(DbProduct.Price.ToString() + "-" + DbProduct.Id.ToString() + "-" + DbProduct.Qty.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductInfo2(string Barcode)
        {
            var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Barcode == Barcode);

            if (DbProduct != null)
                return Json(DbProduct.Price.ToString() + "-" + DbProduct.ProductName.ToString() + "-" + DbProduct.Id.ToString() + "-" + DbProduct.Qty.ToString(), JsonRequestBehavior.AllowGet);
            else
                return Json("Unsuccesfull", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductQty(int ProductId)
        {
            var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Id == ProductId);

            return Json(DbProduct.Qty.ToString(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertInvoicePurchase(string UserBizCode, string TotalPrice)
        {
            var DbUser = _context.Tbl_User.SingleOrDefault(u => u.BIZCode == UserBizCode);
            

                PersianDateTime now = PersianDateTime.Now;
                var ShamsiDate = now.ToString();

                DateTime MiladiDate = DateTime.Now;

                var Invoice = new Tbl_Invoices { UserID = DbUser.Id, SellerID = Convert.ToInt32(Session["UserId"]), DateTime = ShamsiDate, InvoiceStatesID = 1, Description = "خرید از فروشگاه سیرجان" };

                var Purchase = new Tbl_SirjanPurchase { UserId = DbUser.Id, PurchaseId = 1, Price = TotalPrice, DateTime = ShamsiDate,MiladiDate=MiladiDate, Description = "خرید از فروشگاه سیرجان" };
                _context.Tbl_Invoices.Add(Invoice);
                _context.SaveChanges();

                _context.Tbl_SirjanPurchase.Add(Purchase);
                _context.SaveChanges();

                int LastInvoiceId = _context.Database.SqlQuery<int>(@"select top 1 Id from Tbl_Invoices order by Id desc").FirstOrDefault<int>();
                return Json(LastInvoiceId.ToString(), JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult _Profile(string id)
        {
            var DbUser = _context.Tbl_User.SingleOrDefault(u => u.BIZCode == id);

            return PartialView("_Profile", DbUser);
        }

        public PartialViewResult _Invoice(string id)
        {

            return PartialView("_Invoice");
        }

        public JsonResult InsertInvoiceItems(List<Tbl_InvoiceItems> InvoiceItems)
        {
            //Check for NULL.
            if (InvoiceItems == null)
            {
                InvoiceItems = new List<Tbl_InvoiceItems>();
            }

            //Loop and insert records.
            for (int i = 1; i < InvoiceItems.Count(); i++)
            {
                
                int ProductId = InvoiceItems[i].ProductId;

                var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Id == ProductId);
                DbProduct.Qty = Convert.ToInt16(DbProduct.Qty - InvoiceItems[i].Qty);

                _context.Tbl_InvoiceItems.Add(InvoiceItems[i]);
                _context.SaveChanges();
            }

            return Json((InvoiceItems.Count()) - 1);

        }

    }
}