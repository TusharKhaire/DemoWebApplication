using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Models;
using Newtonsoft.Json;
namespace DemoWebApplication.Controllers
{
    public class ItemDetailController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: ItemDetail
        public ActionResult Index()
        {
            List<ItemDetail> itemdetaillist = new List<ItemDetail>();
            var result = dbcon.ItemDetails.ToList();
            foreach (var item in result)
            {
                ItemDetail id = new ItemDetail();
                var itemnamedata = dbcon.ItemMasters.Where(x => x.ItemCode == item.ItemMasterId).FirstOrDefault();
                var itemtypedata = dbcon.ItemTypes.Where(x => x.TypeId == itemnamedata.ItemType).FirstOrDefault();
                var godowndata = dbcon.GodownMasters.Where(x => x.GodownId == item.GodownId).FirstOrDefault();
                var UnitData = dbcon.UnitMasters.Where(x => x.UnitId == item.UnitId).FirstOrDefault();
                id.ItemdetailId = item.ItemdetailId;
                id.ItemMasterId = item.ItemMasterId;
                id.ItemName = itemnamedata.ItemName;
                id.ItemType = itemtypedata.TypeName;
                id.Godown = godowndata.GodownName;
                id.Unit  = UnitData.UnitName ;
                id.BatchId = item.BatchId;
                id.BatchName = item.BatchName;
                id.HsnCode = itemnamedata.HSNCODE;
                id.DiscPer = item.DiscPer;
                id.Expirydate = item.Expirydate;  //.HasValue  ? Formatting(item.Expirydate,"dd/MMM/yyyy") : "");
                id.PurchasePrice = item.PurchasePrice;
                id.MRP = item.MRP;
                id.OpeningStock = item.OpeningStock;
                id.ClosingStock = item.ClosingStock;

                itemdetaillist.Add(id);
            }
            return View(itemdetaillist);
        }
        [HttpGet]
        public ActionResult Create()
        {
            DemoWebApplication.Models.ItemDetailM viewmodel = new DemoWebApplication.Models.ItemDetailM();
            List<SelectListItem> Itemdetailnames = new List<SelectListItem>();
            List<ItemMaster> itemname = dbcon.ItemMasters.ToList();
            itemname.ForEach(x =>
            {
                Itemdetailnames.Add(new SelectListItem { Text = x.ItemName, Value = x.ItemCode.ToString() });
            });
            viewmodel.itemDetailList = Itemdetailnames;
            List<SelectListItem> Itemtypenames = new List<SelectListItem>();
            List<ItemType> itemtype = dbcon.ItemTypes.ToList();
            itemtype.ForEach(x =>
            {
                Itemtypenames.Add(new SelectListItem { Text = x.TypeName, Value = x.TypeId.ToString() });
            });
            viewmodel.lstitemtypes = Itemtypenames;
            List<SelectListItem> Itemunitnames = new List<SelectListItem>();
            List<UnitMaster> unitlist = dbcon.UnitMasters.ToList();
            unitlist.ForEach(x =>
            {
                Itemunitnames.Add(new SelectListItem { Text = x.UnitName, Value = x.UnitId.ToString() });
            });
            viewmodel.lstunits = Itemunitnames;
            List<SelectListItem> Itemgodownnames = new List<SelectListItem>();
            List<GodownMaster> godownlist = dbcon.GodownMasters.ToList();
            godownlist.ForEach(x =>
            {
                Itemgodownnames.Add(new SelectListItem { Text = x.GodownName, Value = x.GodownId.ToString() });
            });
            viewmodel.lstgodowns = Itemgodownnames;
            List<SelectListItem> Itembatchnames = new List<SelectListItem>();
            List<BatchMaster> batchlist = dbcon.BatchMasters.ToList();
            batchlist.ForEach(x =>
            {
                Itembatchnames.Add(new SelectListItem { Text = x.BatchName, Value = x.BatchId.ToString() });
            });
            viewmodel.lstbatches = Itembatchnames;
            viewmodel.mfrdate = new DateTime();
            viewmodel.Expirydate = new DateTime();
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult Create(ItemDetail item)
        {
            if (item != null)
            {
                if (string.IsNullOrEmpty(item.ItemName))
                {
                    ModelState.AddModelError("ItemName", "Please Enter Item Name");
                    return View();
                }
                else if (string.IsNullOrEmpty(item.Godown))
                {
                    ModelState.AddModelError("Godown", "Please Enter Godown");
                    return View();
                }
                else if (item.GodownId < 1)
                {
                    ModelState.AddModelError("Godown", "Please Enter Godown");
                    return View();
                }
                else if (item.OpeningStock < 1)
                {
                    ModelState.AddModelError("OpeningStock", "Please Enter Opening Stock");
                    return View();
                }
            }
            int batchid = Convert.ToInt32(item.BatchName);
            var batchname = dbcon.BatchMasters.Where(x => x.BatchId == batchid).FirstOrDefault();
            ItemDetail newitem = new ItemDetail();
            newitem.ItemMasterId = Convert.ToInt32(item.ItemName);
            newitem.GodownId = Convert.ToInt32(item.Godown);
            newitem.BatchId = Convert.ToInt32(item.BatchName);
            newitem.BatchName = batchname.BatchName;
            newitem.UnitId  = Convert.ToInt32(item.Unit);
            newitem.mfrdate = item.mfrdate;
            newitem.Expirydate = item.Expirydate;
            newitem.PurchasePrice = item.PurchasePrice;
            newitem.MRP = item.MRP;
            newitem.OpeningStock = item.OpeningStock;
            newitem.ClosingStock = item.ClosingStock;
            newitem.DiscPer  = item.DiscPer ;
            dbcon.ItemDetails.Add(newitem);
            dbcon.SaveChanges();
            ViewBag.Message = "Item Save Scessfully";
            ModelState.Clear();
            DemoWebApplication.Models.ItemDetailM viewmodel = new DemoWebApplication.Models.ItemDetailM();
            List<SelectListItem> Itemdetailnames = new List<SelectListItem>();
            List<ItemMaster> itemname = dbcon.ItemMasters.ToList();
            itemname.ForEach(x =>
            {
                Itemdetailnames.Add(new SelectListItem { Text = x.ItemName, Value = x.ItemCode.ToString() });
            });
            viewmodel.itemDetailList = Itemdetailnames;
            List<SelectListItem> Itemtypenames = new List<SelectListItem>();
            //List<ItemType> itemtype = dbcon.ItemTypes.ToList();
            //itemtype.ForEach(x =>
            //{
            //    Itemtypenames.Add(new SelectListItem { Text = x.TypeName, Value = x.TypeId.ToString() });
            //});
            viewmodel.lstitemtypes = Itemtypenames;
            List<SelectListItem> Itemunitnames = new List<SelectListItem>();
            List<UnitMaster> unitlist = dbcon.UnitMasters.ToList();
            unitlist.ForEach(x =>
            {
                Itemunitnames.Add(new SelectListItem { Text = x.UnitName, Value = x.UnitId.ToString() });
            });
            viewmodel.lstunits = Itemunitnames;
            List<SelectListItem> Itemgodownnames = new List<SelectListItem>();
            List<GodownMaster> godownlist = dbcon.GodownMasters.ToList();
            godownlist.ForEach(x =>
            {
                Itemgodownnames.Add(new SelectListItem { Text = x.GodownName, Value = x.GodownId.ToString() });
            });
            viewmodel.lstgodowns = Itemgodownnames;
            List<SelectListItem> Itembatchnames = new List<SelectListItem>();
            List<BatchMaster> batchlist = dbcon.BatchMasters.ToList();
            batchlist.ForEach(x =>
            {
                Itembatchnames.Add(new SelectListItem { Text = x.BatchName, Value = x.BatchId.ToString() });
            });
            viewmodel.lstbatches = Itembatchnames;
            viewmodel.mfrdate = new DateTime();
            viewmodel.Expirydate = new DateTime();
            return View(viewmodel);
        }
        public JsonResult GetItemByName(string searchText)
        {
            var ItemName = dbcon.ItemMasters.Where(a => a.ItemName.Contains(searchText)).ToList();
            return Json(ItemName.Select(q => new { id = q.ItemCode, text = q.ItemName }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetItemTypeData(int itemcode)
        {
            var itemmaster = dbcon.ItemMasters.Where(x => x.ItemCode == itemcode).FirstOrDefault();
            var itemtype = dbcon.ItemTypes.Where(x => x.TypeId == itemmaster.ItemType).FirstOrDefault();
            return Json(itemtype, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetitemDetails()
        {
            var itemtype = dbcon.ItemTypes.ToList();
            //return Json(itemtype, JsonRequestBehavior.AllowGet);
            ItemDetailM id = new ItemDetailM();
            id.ItemdetailId = 1;
            id.ItemName = "ItemName";
            id.ItemType = "Reguler";
            id.DiscPer = 12;
            var result = JsonConvert.SerializeObject(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}