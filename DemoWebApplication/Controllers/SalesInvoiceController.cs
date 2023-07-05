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
        public ActionResult Create()
        {
            SalesInvoice sm = new SalesInvoice();
            sm.Billno = BillNumber();
            return View(sm);
        }
    }
}