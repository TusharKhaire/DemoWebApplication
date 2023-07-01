using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using DemoWebApplication.Models;
using Newtonsoft.Json;
using System.Net;
using System.Data;

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
                id.Unit = UnitData.UnitName;
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
            int batchid=0;
            string batchname="" ; 
            if (int.TryParse(item.BatchName, out int n))
            {
                 batchid = Convert.ToInt32(item.BatchName);
                var chk_batchname = dbcon.BatchMasters.Where(x => x.BatchId == batchid).FirstOrDefault();
                if (chk_batchname != null)
                {
                    batchname = chk_batchname.BatchName;
                }
                else
                {
                    BatchMaster bm = new BatchMaster();
                    bm.BatchName = item.BatchName;
                    bm.Discription = "";
                    dbcon.BatchMasters.Add(bm);
                    dbcon.SaveChanges();
                     chk_batchname = dbcon.BatchMasters.Where(x => x.BatchName== item.BatchName).FirstOrDefault();
                    {
                        batchid  = chk_batchname.BatchId ;
                        batchname = chk_batchname.BatchName;
                    }
                }
            }
            else
            {
                BatchMaster bm = new BatchMaster();
                bm.BatchName = item.BatchName;
                bm.Discription = "";
                dbcon.BatchMasters.Add(bm);
                dbcon.SaveChanges();
               var chk_batchname = dbcon.BatchMasters.Where(x => x.BatchName == item.BatchName).FirstOrDefault();
                {
                    batchid = chk_batchname.BatchId;
                    batchname = chk_batchname.BatchName;
                }
            }
            ItemDetail newitem = new ItemDetail();
            newitem.ItemMasterId = Convert.ToInt32(item.ItemName);
            newitem.GodownId = Convert.ToInt32(item.Godown);
            newitem.BatchId = batchid;//Convert.ToInt32(item.BatchName);
            newitem.BatchName = batchname;
            newitem.UnitId = Convert.ToInt32(item.Unit);
            newitem.mfrdate = item.mfrdate;
            newitem.Expirydate = item.Expirydate;
            newitem.PurchasePrice = item.PurchasePrice;
            newitem.MRP = item.MRP;
            newitem.OpeningStock = item.OpeningStock;
            newitem.ClosingStock = item.ClosingStock;
            newitem.DiscPer = item.DiscPer;
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
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemDetail item_detail = dbcon.ItemDetails.Find(id);
            if (item_detail == null)
            {
                return HttpNotFound();
            }
            DemoWebApplication.Models.ItemDetailM viewmodel =new DemoWebApplication.Models.ItemDetailM();
            List<SelectListItem> itemDetailName = new List<SelectListItem>();
            List<ItemMaster> itemNames = dbcon.ItemMasters.ToList();
            itemNames.ForEach(x => {
                itemDetailName.Add(new SelectListItem {Text=x.ItemName,Value=x.ItemCode.ToString() });
            });
            List<SelectListItem> GodownNameLists = new List<SelectListItem>();
            List<GodownMaster> GodownNames = dbcon.GodownMasters.ToList();
            GodownNames.ForEach(x =>
            {
                GodownNameLists.Add(new SelectListItem { Text = x.GodownName, Value = x.GodownId.ToString() });
            });
            List<SelectListItem>UnitListItems =new List<SelectListItem>();
            List<UnitMaster> UnitNames = dbcon.UnitMasters.ToList();
            UnitNames.ForEach(x =>
            {
                UnitListItems.Add(new SelectListItem { Text = x.UnitName, Value = x.UnitId.ToString() });
            });
            List<SelectListItem>BatchListItem =new List<SelectListItem>();
            List<BatchMaster> BatchNames = dbcon.BatchMasters.ToList();
            BatchNames.ForEach(x =>
            {
                BatchListItem.Add(new SelectListItem { Text = x.BatchName, Value = x.BatchId.ToString() });
            });
            viewmodel.itemDetailList = itemDetailName;
            viewmodel.lstunits = UnitListItems;
            viewmodel.lstgodowns = GodownNameLists;
            viewmodel.lstbatches = BatchListItem;
            viewmodel.ItemdetailId = item_detail.ItemdetailId;
            viewmodel.ItemMasterId = item_detail.ItemMasterId;
            viewmodel.BatchId = item_detail.BatchId;
            viewmodel.BatchName = item_detail.BatchName;
            viewmodel.GodownId = item_detail.GodownId;
            viewmodel.UnitId = item_detail.UnitId;
            viewmodel.DiscPer = item_detail.DiscPer;
            viewmodel.mfrdate = item_detail.mfrdate;
            viewmodel.Expirydate  = item_detail.Expirydate;
            viewmodel.PurchasePrice = item_detail.PurchasePrice;
            viewmodel.MRP = item_detail.MRP ;
            viewmodel.OpeningStock = item_detail.OpeningStock;
            viewmodel.ClosingStock = item_detail.ClosingStock;
            
            return View("Create",viewmodel);
        }
        [HttpPost]
        public ActionResult Edit(ItemDetail Item)
        {
            return View();
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
        public JsonResult GetBatchDetails(string searchText)
        {
            var Batch = dbcon.BatchMasters.Where(a => a.BatchName.Contains(searchText)).ToList();
            return Json(Batch.Select(q => new { id = q.BatchId, text = q.BatchName }), JsonRequestBehavior.AllowGet);
            //return Json("",JsonRequestBehavior .AllowGet);
        }
    }

}