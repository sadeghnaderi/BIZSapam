using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Data;
using BizSapam.Models;
using BizSapam.ViewModels;
using System.IO;

namespace BizSapam.Controllers
{
    public class AdminController : Controller
    {
        private MyDBContext _context;
        public AdminController()
        {
            _context = new MyDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashbord()
        {
            if (Session["UserId"] != null)
            {
                List<UserShopingInfo> UserList = (_context.Database.SqlQuery<UserShopingInfo>(@"select TOP 30 Tbl_User.Name , Tbl_User.Id ,
            sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 1
                THEN CAST(Tbl_SirjanPurchase.Price AS bigint)
            ELSE 0
            END) AS [TotalPurchase]
			
         from Tbl_User left join Tbl_SirjanPurchase on Tbl_User.Id = Tbl_SirjanPurchase.UserId
         where Tbl_User.AccessLevelId=3
         group by Tbl_User.Name,Tbl_User.Id
         ORDER BY TotalPurchase DESC").ToList());

            UserShopingInfoList UsersList = new UserShopingInfoList
            {
                UsersInfo = UserList
            };

            return View(UsersList);
            }
            else
                return RedirectToAction("Login", "Home");
        }

        public ActionResult ManageProduct(string search, int? i)
        {
                if (Session["UserId"] != null)
                {
                    List<Tbl_Products> ProductList = _context.Tbl_Products.ToList();

            if (search == "AllProducts")
            {
                var viewmodel = new ProductIpagedList
                {
                    IpagedListProduct = ProductList.ToList().ToPagedList(i ?? 1, 50)
                };

                return View(viewmodel);
            }
            else
            {
                var viewmodel = new ProductIpagedList
                {
                    IpagedListProduct = ProductList.Where(x => x.ProductName.Contains(search) || search == null).ToList().ToPagedList(i ?? 1, 50)
                };
                return View(viewmodel);
            }
                }
                else
                    return RedirectToAction("Login", "Home");
            }
        [HttpPost]
        public ActionResult AddProduct(ProductIpagedList ProductIpagedList)
        {
            if (Session["UserId"] != null)
            {
                if (ProductIpagedList.Products.Id == 0)
                _context.Tbl_Products.Add(ProductIpagedList.Products);
            else
            {
                var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Id == ProductIpagedList.Products.Id);
                DbProduct.ProductName = ProductIpagedList.Products.ProductName;
                DbProduct.Price = ProductIpagedList.Products.Price;
                DbProduct.Qty = ProductIpagedList.Products.Qty;
                DbProduct.Description = ProductIpagedList.Products.Description;
                DbProduct.Barcode = ProductIpagedList.Products.Barcode;
            }
            _context.SaveChanges();
            return RedirectToAction("ManageProduct", "Admin", new { search = "AllProducts" });
            }
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public JsonResult DeleteProduct(int ProductId)
        {
            
            var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Id == ProductId);
            _context.Tbl_Products.Remove(DbProduct);
            _context.SaveChanges();
            return Json("Successful", JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditProduct(int id)
        {
                if (Session["UserId"] != null)
                {
                    var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Id == id);
            List<Tbl_Products> ProductList = _context.Tbl_Products.ToList();

            var ViewModel = new ProductIpagedList
            {
                Products = DbProduct,
                IpagedListProduct = ProductList.ToList().ToPagedList(1, 10)
            };

            return View("ManageProduct", ViewModel);
                }
                else
                    return RedirectToAction("Login", "Home");
            }

        [HttpPost]
        public JsonResult DeleteInvoice(int InvoiceId)
        {

            List<Tbl_InvoiceItems> DbInvoiceItemsList = _context.Tbl_InvoiceItems.Where(u => u.InvoiceId == InvoiceId).ToList();

            foreach(var InvoiceItem in DbInvoiceItemsList)
            {
                _context.Tbl_InvoiceItems.Remove(InvoiceItem);
            }

            var DbInvoice = _context.Tbl_Invoices.SingleOrDefault(u => u.Id == InvoiceId);
            var DbPurchase = _context.Tbl_SirjanPurchase.SingleOrDefault(u => u.DateTime == DbInvoice.DateTime);

            _context.Tbl_Invoices.Remove(DbInvoice);
            _context.Tbl_SirjanPurchase.Remove(DbPurchase);

            _context.SaveChanges();
            return Json("Successful", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ManageUser(string search, int? i)
        {
                    if (Session["UserId"] != null)
                    {
                        List<Tbl_User> UserList = _context.Tbl_User.ToList();
            List<Tbl_AccessLevels> AccessLevelList = _context.Tbl_AccessLevels.ToList();

            if (search == "AllUsers")
            {
                var viewmodel = new UserIpagedList
                {
                    IpagedListUser = UserList.Where(x => x.AccessLevelID != 4).ToList().ToPagedList(i ?? 1, 20),
                    AccessLevels = AccessLevelList
                };

                return View(viewmodel);
            }
            else
            {
                var viewmodel = new UserIpagedList
                {
                    IpagedListUser = UserList.Where(x => x.Name.Contains(search) || search == null).ToList().ToPagedList(i ?? 1, 20),
                    AccessLevels = AccessLevelList
                };
                return View(viewmodel);
            }
                    }
                    else
                        return RedirectToAction("Login", "Home");
                }

        [HttpPost]
        public ActionResult AddUser(UserIpagedList UserIpagedList, HttpPostedFileBase file)
        {
            if (Session["UserId"] != null)
            {
                string Picture = Guid.NewGuid().ToString();

            if (file.ContentLength > 0)
            {
                var path = Path.Combine(Server.MapPath("~/images/Users"), Picture + ".jpg");
                file.SaveAs(path);
            }

            if (UserIpagedList.Users.Id == 0)
            {
                UserIpagedList.Users.Picture = Picture + ".jpg";
                _context.Tbl_User.Add(UserIpagedList.Users);
            }
            else
            {
                var DbUser = _context.Tbl_User.SingleOrDefault(u => u.Id == UserIpagedList.Users.Id);
                DbUser.Name = UserIpagedList.Users.Name;
                DbUser.BIZCode = UserIpagedList.Users.BIZCode;
                DbUser.Username = UserIpagedList.Users.Username;
                DbUser.Password = UserIpagedList.Users.Password;
                DbUser.Picture = Picture + ".jpg";
                DbUser.AccessLevelID = UserIpagedList.Users.AccessLevelID;

            }
            _context.SaveChanges();
            return RedirectToAction("ManageUser", "Admin", new { search = "AllUsers" });
            }
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public JsonResult DeleteUser(int UserId)
        {
            
                var DbUser = _context.Tbl_User.SingleOrDefault(u => u.Id == UserId);
            _context.Tbl_User.Remove(DbUser);
            _context.SaveChanges();
            return Json("Successful", JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditUser(int id)
        {
                if (Session["UserId"] != null)
                {
                    var DbUser = _context.Tbl_User.SingleOrDefault(u => u.Id == id);
            List<Tbl_User> UserList = _context.Tbl_User.ToList();
            List<Tbl_AccessLevels> AccessLevelList = _context.Tbl_AccessLevels.ToList();

            var ViewModel = new UserIpagedList
            {
                Users = DbUser,
                IpagedListUser = UserList.ToList().ToPagedList(1, 10),
                AccessLevels = AccessLevelList

            };

            return View("ManageUser", ViewModel);
                }
                else
                    return RedirectToAction("Login", "Home");
            }

        public ActionResult UsersSellInformation(string search, int? i)
        {
                    if (Session["UserId"] != null)
                    {
                        List<UserShopingInfo> UserList = (_context.Database.SqlQuery<UserShopingInfo>(@"select Tbl_User.Name , Tbl_User.Id ,
 sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 1
                THEN CAST(Tbl_SirjanPurchase.Price AS bigint)
            ELSE 0
            END)AS [TotalPurchase]
			, 
 CAST((sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 3
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END) -  sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 4
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END))AS varchar(10)) AS [TotalBIZCredit]
			,
 CAST((sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 3
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END) +  sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 1
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END) -  sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 2
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END))AS varchar(10)) AS [TehranPurchase]
			,
 CAST((sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 2
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END) -  sum(CASE
            WHEN Tbl_SirjanPurchase.PurchaseId = 1
                THEN CAST(Tbl_SirjanPurchase.Price AS int)
            ELSE 0
            END))AS varchar(10)) AS [MaxPurchaseAllowed]
 from Tbl_User left join Tbl_SirjanPurchase on Tbl_User.Id = Tbl_SirjanPurchase.UserId
 where Tbl_User.AccessLevelId=3
 group by Tbl_User.Name,Tbl_User.Id").ToList());

            if (search == "AllUsers")
            {
                var viewmodel = new UserInformationsIpagedList
                {
                    IpagedListUserInfo = UserList.ToList().ToPagedList(i ?? 1, 30)
                };
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new UserInformationsIpagedList
                {
                    IpagedListUserInfo = UserList.Where(x => x.Name.Contains(search) || search == null).ToList().ToPagedList(i ?? 1, 10)
                };
                return View(viewmodel);
            }

                    }
                    else
                        return RedirectToAction("Login", "Home");

                }

        public ActionResult UsersSellInfoList( int? i , string FromDate , string ToDate)
        {
                        if (Session["UserId"] != null)
                        {
                            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['/'] = '/',['۰'] = '0',['۱'] = '1',['۲'] = '2',['۳'] = '3',['۴'] = '4',['۵'] = '5',['۶'] = '6',['۷'] = '7',['۸'] = '8',['۹'] = '9',['0'] = '0',['1'] = '1',['2'] = '2',['3'] = '3',['4'] = '4',['5'] = '5', ['6'] = '6',['7'] = '7',['8'] = '8',['9'] = '9'
            };
            foreach (var item in FromDate)
            {
                FromDate = FromDate.Replace(item, LettersDictionary[item]);
            }
            foreach (var item in ToDate)
            {
                ToDate = ToDate.Replace(item, LettersDictionary[item]);
            }

            PersianDateTime FromD = PersianDateTime.Parse(FromDate);
            DateTime miladiFromD = FromD.ToDateTime();

            PersianDateTime ToD = PersianDateTime.Parse(ToDate);
            DateTime miladiToD = ToD.ToDateTime();

            List<UserShopingInfo> UserList = (_context.Database.SqlQuery<UserShopingInfo>(@"select Tbl_User.Name , Tbl_User.Id , Tbl_User.BIZCode ,sum(CAST(Tbl_SirjanPurchase.Price AS bigint)) AS [TotalPurchase] from Tbl_User inner join Tbl_SirjanPurchase on Tbl_User.Id = Tbl_SirjanPurchase.UserId WHERE Tbl_SirjanPurchase.PurchaseId = 1 and Tbl_SirjanPurchase.MiladiDate > '" + miladiFromD + "' and Tbl_SirjanPurchase.MiladiDate< '" + miladiToD + "' group by Tbl_User.Name,Tbl_User.Id,Tbl_User.BIZCode")).ToList();

            string TotalPrice = (_context.Database.SqlQuery<string>(@"SELECT CAST(sum(CAST(Tbl_SirjanPurchase.Price AS int))AS varchar) AS [TotalPrice] from Tbl_User inner join Tbl_SirjanPurchase on Tbl_User.Id = Tbl_SirjanPurchase.UserId WHERE Tbl_SirjanPurchase.PurchaseId = 1 and Tbl_SirjanPurchase.MiladiDate > '" + miladiFromD+ "' and Tbl_SirjanPurchase.MiladiDate< '" + miladiToD+"'")).FirstOrDefault<string>();

            ViewBag.TotalPrice = TotalPrice;

            var viewmodel = new UserInformationsIpagedList
                {
                    IpagedListUserInfo = UserList.ToList().ToPagedList(i ?? 1, 100000)
                };
                return View(viewmodel);

                        }
                        else
                            return RedirectToAction("Login", "Home");
                    }


        public ActionResult SirjanShopSellInformations(string search, int? i)
        {
                            if (Session["UserId"] != null)
                            {
                                List<Tbl_Invoices> InvoiceList = _context.Tbl_Invoices.OrderByDescending(x => x.Id).ToList();

            if (search == "AllInvoice")
            {
                var viewmodel = new InvoiceIpagedList
                {
                    IpagedListInvoice = InvoiceList.ToList().ToPagedList(i ?? 1, 50),
                };

                return View(viewmodel);
            }
            else
            {
                var viewmodel = new InvoiceIpagedList
                {
                    IpagedListInvoice = InvoiceList.Where(x => x.Tbl_User.Name.Contains(search) || search == null).ToList().ToPagedList(i ?? 1, 50),
                };
                return View(viewmodel);
            }
                            }
                            else
                                return RedirectToAction("Login", "Home");
                        }

        public ActionResult InvoiceDetails(int id)
        {
                                if (Session["UserId"] != null)
                                {
            List<Tbl_InvoiceItems> InvoiceItemsList = _context.Tbl_InvoiceItems.Where(x => x.InvoiceId == id).ToList();

            var viewmodel = new InvoiceItems
            {
                InvoiceItem = InvoiceItemsList
            };

            return View(viewmodel);
                                    }
            else
                return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public JsonResult InsertUserBalance(string UserId, string BalancePrice, string Temp)
        {
            
                PersianDateTime now = PersianDateTime.Now;
            var DateTime = now.ToString();

            var Purchase = new Tbl_SirjanPurchase { };

            if (Temp == "Increase")
            {
                Purchase = new Tbl_SirjanPurchase { UserId = Convert.ToInt32(UserId), PurchaseId = 3, Price = BalancePrice, DateTime = DateTime, Description = "افزایش سهام کاربر" };
            }
            else if (Temp == "Decrease")
            {
                Purchase = new Tbl_SirjanPurchase { UserId = Convert.ToInt32(UserId), PurchaseId = 4, Price = BalancePrice, DateTime = DateTime, Description = "کاهش سهام کاربر" };
            }
            _context.Tbl_SirjanPurchase.Add(Purchase);
            _context.SaveChanges();
            return Json("Successful", JsonRequestBehavior.AllowGet);
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
        public ActionResult IncomeProducts()
        {
            if (Session["UserId"] != null)
            {
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

            return Json(DbProduct.Price.ToString() + "-" + DbProduct.Id.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductInfo2(string Barcode)
        {
            
                var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Barcode == Barcode);

            if (DbProduct != null)
                return Json(DbProduct.Price.ToString() + "-" + DbProduct.ProductName.ToString() + "-" + DbProduct.Id.ToString(), JsonRequestBehavior.AllowGet);
            else
                return Json("Unsuccesfull", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertInvoice()
        {
            
                PersianDateTime now = PersianDateTime.Now;
            var DateTime = now.ToString();

            var Invoice = new Tbl_Invoices { UserID = 1007, SellerID = 1008, DateTime = DateTime, InvoiceStatesID = 1, Description = "خرید از فروشگاه تهران" };

            _context.Tbl_Invoices.Add(Invoice);
            _context.SaveChanges();
            
            int LastInvoiceId = _context.Database.SqlQuery<int>(@"select top 1 Id from Tbl_Invoices order by Id desc").FirstOrDefault<int>();
            
            return Json(LastInvoiceId, JsonRequestBehavior.AllowGet);
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
                //problem is here
                int ProductId = InvoiceItems[i].ProductId;
                var DbProduct = _context.Tbl_Products.SingleOrDefault(u => u.Id == ProductId);
                //........

                DbProduct.Qty = Convert.ToInt16(DbProduct.Qty + InvoiceItems[i].Qty);

                _context.Tbl_InvoiceItems.Add(InvoiceItems[i]);

                _context.SaveChanges();
            }

            return Json((InvoiceItems.Count()) - 1);
                
            }



    }


}


