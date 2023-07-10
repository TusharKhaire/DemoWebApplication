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
            return billno ?? 1;
        }
        public ActionResult CreateInvoice()
        {
            ViewBag.Message = null;
            SalesInvoice viewmodel = new SalesInvoice();
            List<SelectListItem> lst_Cust = new List<SelectListItem>();
            List<AccountMaster> custNamelist = dbcon.AccountMasters.ToList();
            custNamelist.ForEach(x =>
            {
                lst_Cust.Add(new SelectListItem { Text = x.AccountName, Value = x.AccountCode.ToString() });
            });
            viewmodel.lst_CustomerNameList = lst_Cust;
            List<SelectListItem> lst_Item = new List<SelectListItem>();
            var itemnames = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).ToList();
            itemnames.ForEach(x =>
            {
                lst_Item.Add(new SelectListItem { Text = x.ItemMaster.ItemName, Value = x.ItemDetail.ItemdetailId.ToString() });
            });
            viewmodel.lst_ItemName = lst_Item;
            List<SelectListItem> stateList = new List<SelectListItem>();
            stateList.Add(new SelectListItem { Text = "Maharashtra", Value = "1" });
            stateList.Add(new SelectListItem { Text = "MP", Value = "2" });
            stateList.Add(new SelectListItem { Text = "AP", Value = "3" });
            stateList.Add(new SelectListItem { Text = "UP", Value = "4" });
            stateList.Add(new SelectListItem { Text = "Goa", Value = "5" });
            viewmodel.lst_State = stateList;
            viewmodel.Billno = BillNumber();
            viewmodel.Invoicedate = System.DateTime.Now;
            viewmodel.Duedate = DateTime.Now;
            viewmodel.CustState = "Maharashtra";
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult CreateInvoice(SalesInvoice salesData)
        {
            return RedirectToAction("CreateInvoice");
        }

        public JsonResult GetCustomerName(string searchText)
        {
            var Customer = dbcon.AccountMasters.Where(x => x.AccountName.Contains(searchText)).ToList();
            return Json(Customer.Select(x => new { id = x.AccountCode, text = x.AccountName }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerData(int id)
        {
            var Customerdata = dbcon.AccountMasters.Where(x => x.AccountCode == id).FirstOrDefault();
            return Json(Customerdata, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemName(string searchText)
        {
            var ItemName = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Select(i => new
            {
                ItemCode = i.ItemDetail.ItemdetailId,
                ItemName = i.ItemMaster.ItemName,
                Details = i.ItemDetail.ItemdetailId + ":" + i.ItemMaster.ItemName + ":" + i.ItemDetail.ClosingStock
            }).Where(x => x.ItemName.Contains(searchText)).ToList();
            return Json(ItemName.Select(x => new { id = x.ItemCode, text = x.Details }), JsonRequestBehavior.AllowGet);
        }
        public class itemdata
        {
            public int Itemmasterid;
            public int Itemdetailid;
            public string Itemname;
            //public int BatchId;            
            public string Batch;
            //public int GodownId;
            public string Godown;
            public string ItemType;
            //public int MfrId;
            public int mrp;
            public int purchaseprice;
            public string expdate;
            public double closingstock;
            public string HSNCOde;
            public double Gst;
        }
        public JsonResult GetItemByName(string searchText)
        {
            List<itemdata> lst_Itemdata = new List<itemdata>();
            var ItemData = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Where(x => x.ItemMaster.ItemName.Contains(searchText)).ToList();
            foreach (var item in ItemData)
            {
                itemdata obj = new itemdata();
                var batch = dbcon.BatchMasters.Where(x => x.BatchId == item.ItemDetail.BatchId).FirstOrDefault();
                var Godown = dbcon.GodownMasters.Where(x => x.GodownId == item.ItemDetail.GodownId).FirstOrDefault();
                var Type = dbcon.ItemTypes.Where(x => x.TypeId == item.ItemMaster.ItemType).FirstOrDefault();
                obj.Itemdetailid = item.ItemDetail.ItemdetailId;
                obj.Itemmasterid = item.ItemMaster.ItemCode;
                obj.Itemname = item.ItemMaster.ItemName;
                obj.ItemType = Type.TypeName;
                obj.Batch = batch.BatchName;
                obj.Godown = Godown.GodownName;
                obj.mrp = item.ItemDetail.MRP ?? 0;
                obj.purchaseprice = item.ItemDetail.PurchasePrice ?? 0;
                obj.closingstock = item.ItemDetail.ClosingStock ?? 0;
                lst_Itemdata.Add(obj);
            }

            return Json(lst_Itemdata.Select(x => new { id = x.Itemdetailid, text = x.Itemname + ":" + x.ItemType + ":" + x.Batch + ":" + x.Godown + ":" + x.closingstock }), JsonRequestBehavior.AllowGet);  // 
        }
        public JsonResult GetItemsData(int code)
        {
            var item = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Where(x => x.ItemDetail.ItemdetailId == code).FirstOrDefault();

            itemdata obj = new itemdata();
            var batch = dbcon.BatchMasters.Where(x => x.BatchId == item.ItemDetail.BatchId).FirstOrDefault();
            var Godown = dbcon.GodownMasters.Where(x => x.GodownId == item.ItemDetail.GodownId).FirstOrDefault();
            var Type = dbcon.ItemTypes.Where(x => x.TypeId == item.ItemMaster.ItemType).FirstOrDefault();
            obj.Itemdetailid = item.ItemDetail.ItemdetailId;
            obj.Itemmasterid = item.ItemMaster.ItemCode;
            obj.Itemname = item.ItemMaster.ItemName;
            obj.ItemType = Type.TypeName;
            obj.Batch = batch.BatchName;
            obj.Godown = Godown.GodownName;
            obj.mrp = item.ItemDetail.MRP ?? 0;
            obj.purchaseprice = item.ItemDetail.PurchasePrice ?? 0;
            obj.closingstock = item.ItemDetail.ClosingStock ?? 0;
            obj.HSNCOde = item.ItemMaster.HSNCODE;
            obj.Gst =Convert.ToDouble( item.ItemMaster.GstPer);
            string formattedDateString = item.ItemDetail.Expirydate?.ToString("dd-MMM-yyyy");
            obj.expdate = formattedDateString;
            //obj.expdate = item.ItemDetail.Expirydate?? 0;   
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}