using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Models;

namespace DemoWebApplication.Controllers
{
    public class SalesInvoiceController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: SalesInvoice
        public ActionResult Index()
        {
            return View();
        }
        private int BillNumber()
        {
            int? billno = 0;
            billno = dbcon.SalesInvoiceMasters.Max(x => (int?)x.Billno);
            billno = billno.HasValue ? billno + 1 : 1;
            return billno??1;
        }
        public ActionResult CreateInvoice()
        {
            ViewBag.Message = null;
            SalesInvoice viewmodel = new SalesInvoice();
            List<SelectListItem> lst_Cust = new List<SelectListItem>();
            List<AccountMaster> custNamelist = dbcon.AccountMasters.ToList();
            custNamelist.ForEach(x =>
            {
                lst_Cust.Add(new SelectListItem {Text=x.AccountName,Value=x.AccountCode.ToString() });
            });
            viewmodel.lst_CustomerNameList = lst_Cust;
            List<SelectListItem> lst_Item = new List<SelectListItem>();
            var itemnames = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).ToList();
            itemnames.ForEach(x =>
            {
                lst_Item.Add(new SelectListItem {Text=x.ItemMaster.ItemName,Value=x.ItemDetail.ItemdetailId .ToString() });
            });
            viewmodel.lst_ItemName = lst_Item;
            viewmodel.Billno = BillNumber();
            viewmodel.Invoicedate = System.DateTime.Now;
            viewmodel.Duedate  = DateTime.Now;
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult CreateInvoice(SalesInvoice salesData)
        {
            return RedirectToAction("CreateInvoice");
        }
    }
}