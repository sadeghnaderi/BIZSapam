using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BizSapam.Models
{
    public class MyDBContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MyDBContext>(null);
            base.OnModelCreating(modelBuilder);
        }
        public MyDBContext() : base("name=BIZConnectionString")
        {

        }
        public DbSet<Tbl_AccessLevels> Tbl_AccessLevels { get; set; }
        public DbSet<Tbl_InvoiceItems> Tbl_InvoiceItems { get; set; }
        public DbSet<Tbl_Invoices> Tbl_Invoices { get; set; }
        public DbSet<Tbl_InvoiceStates> Tbl_InvoiceStates { get; set; }
        public DbSet<Tbl_LogTypes> Tbl_LogTypes { get; set; }
        public DbSet<Tbl_Products> Tbl_Products { get; set; }
        public DbSet<Tbl_PurchaseTypes> Tbl_PurchaseTypes { get; set; }
        public DbSet<Tbl_SirjanPurchase> Tbl_SirjanPurchase { get; set; }
        public DbSet<Tbl_TehranPurchase> Tbl_TehranPurchase { get; set; }
        public DbSet<Tbl_User> Tbl_User { get; set; }
        public DbSet<Tbl_UserLogs> Tbl_UserLogs { get; set; }

    }
}