using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizSapam.Models;

namespace BizSapam.Controllers
{
    public class HomeController : Controller
    {
        private MyDBContext _context;
        public HomeController()
        {
            _context = new MyDBContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if (Request.QueryString["Logout"] == "True")
                Session["UserId"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult CheckUser(Tbl_User User)
        {
            var DbUser = _context.Tbl_User.SingleOrDefault(u => u.Username == User.Username);

            if (DbUser == null || DbUser.Password != User.Password)
            {
                ViewBag.Error = "نام کاربری و یا رمز عبور اشتباه است";
                return View("Login");
            }
            else if (DbUser.Password == User.Password)
            {
                ViewBag.Error = "";
                Session["UserId"] = DbUser.Id;

                if (DbUser.AccessLevelID == 1)
                    return RedirectToAction("Dashbord", "Admin");
                else if (DbUser.AccessLevelID == 2)
                    return RedirectToAction("Home", "SellerPanel");
                else
                    return RedirectToAction("Login", "Home");
            }
            else
                return HttpNotFound();
        }

    }
}