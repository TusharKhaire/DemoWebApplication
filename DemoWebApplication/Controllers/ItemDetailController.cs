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
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace DemoWebApplication.Controllers
{
    public class ItemDetailController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: ItemDetail
        public ActionResult Index(string search,int ? i)
        {
            
            List<ItemDetail> itemdetaillist = new List<ItemDetail>();
            var result = dbcon.ItemDetails.ToList();
            var itemtypesdata = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMasters = a, ItemDetail = b }).Where(ab => ab.ItemMasters.ItemCode == ab.ItemDetail.ItemMasterId).Select(ab => ab.ItemMasters.ItemName).Distinct();    //
            var itemData = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Where(x=>x.ItemMaster.ItemName.StartsWith(search)||search==null).ToList();

            var newItemData = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId,
        (a, b) => new { ItemMaster = a, ItemDetail = b }).Select(ab => new
        {
            ItemName = ab.ItemMaster.ItemName,
            ItemDetailsId = ab.ItemDetail.ItemdetailId ,
            ItemMasterId = ab.ItemDetail.ItemMasterId
        }).ToList();
            foreach (var item in itemData)    // change result to itemData for working with joins operation
            {
                ItemDetail id = new ItemDetail();
                var itemnamedata = dbcon.ItemMasters.Where(x => x.ItemCode == item.ItemDetail.ItemMasterId).FirstOrDefault();
                var itemtypedata = dbcon.ItemTypes.Where(x => x.TypeId == itemnamedata.ItemType).FirstOrDefault();
                //var itemtypesdata = dbcon.ItemMasters.Join(dbcon.ItemTypes, a => a.ItemCode, b => b.TypeId, (a, b) => new { ItemMasters = a, ItemTypes = b }).Where(ab => ab.ItemMasters.ItemCode == ab.ItemTypes.TypeId).Select(ab => ab.ItemTypes.TypeName).Distinct();
                var godowndata = dbcon.GodownMasters.Where(x => x.GodownId == item.ItemDetail.GodownId).FirstOrDefault();
                var UnitData = dbcon.UnitMasters.Where(x => x.UnitId == item.ItemDetail.UnitId).FirstOrDefault();
                id.ItemdetailId = item.ItemDetail.ItemdetailId;
                id.ItemMasterId = item.ItemDetail.ItemMasterId;
                id.ItemName = item.ItemMaster.ItemName;
                id.ItemType = itemtypedata.TypeName;
                //var type= itemtypesdata.Select(data => data.TypeName);
                //id.ItemType = type;
                id.Godown = godowndata.GodownName;
                id.Unit = UnitData.UnitName;
                id.BatchId = item.ItemDetail.BatchId;
                id.BatchName = item.ItemDetail.BatchName;
                id.HsnCode = itemnamedata.HSNCODE;
                id.DiscPer = item.ItemDetail.DiscPer;
                id.Expirydate = item.ItemDetail.Expirydate;  //.HasValue  ? Formatting(item.Expirydate,"dd/MMM/yyyy") : "");
                id.PurchasePrice = item.ItemDetail.PurchasePrice;
                id.MRP = item.ItemDetail.MRP;
                id.OpeningStock = item.ItemDetail.OpeningStock;
                id.ClosingStock = item.ItemDetail.ClosingStock;
                
                itemdetaillist.Add(id);
            }
            return View(itemdetaillist.ToPagedList(i ?? 1, 4));
        }
        //private ItemDetail GetItemsList(int currentPage)
        //{

        //}
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
            //viewmodel.mfrdate = DateTime.Now;
            //viewmodel.Expirydate = DateTime.Now;

            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult Create(ItemDetail item)
        {
            if (item != null)
            {
                if (item.ItemMasterId < 1)
                {
                    ModelState.AddModelError("ItemName", "Please Enter Item Name");
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
            int batchid = 0;
            string batchname = "";
            if (item.BatchId != null)
            {
                batchid = item.BatchId.HasValue ? (int)item.BatchId : 0;
                batchname = item.BatchName;
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
            //newitem.ItemMasterId = Convert.ToInt32(item.ItemName);
            newitem.ItemMasterId = item.ItemMasterId;
            newitem.GodownId = item.GodownId;
            newitem.BatchId = batchid;//Convert.ToInt32(item.BatchName);
            newitem.BatchName = batchname;
            newitem.UnitId = item.UnitId;
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
            DemoWebApplication.Models.ItemDetailM viewmodel = new DemoWebApplication.Models.ItemDetailM();
            List<SelectListItem> itemDetailName = new List<SelectListItem>();
            List<ItemMaster> itemNames = dbcon.ItemMasters.ToList();
            itemNames.ForEach(x =>
            {
                itemDetailName.Add(new SelectListItem { Text = x.ItemName, Value = x.ItemCode.ToString() });
            });
            List<SelectListItem> GodownNameLists = new List<SelectListItem>();
            List<GodownMaster> GodownNames = dbcon.GodownMasters.ToList();
            GodownNames.ForEach(x =>
            {
                GodownNameLists.Add(new SelectListItem { Text = x.GodownName, Value = x.GodownId.ToString() });
            });
            List<SelectListItem> UnitListItems = new List<SelectListItem>();
            List<UnitMaster> UnitNames = dbcon.UnitMasters.ToList();
            UnitNames.ForEach(x =>
            {
                UnitListItems.Add(new SelectListItem { Text = x.UnitName, Value = x.UnitId.ToString() });
            });
            List<SelectListItem> BatchListItem = new List<SelectListItem>();
            List<BatchMaster> BatchNames = dbcon.BatchMasters.ToList();
            BatchNames.ForEach(x =>
            {
                BatchListItem.Add(new SelectListItem { Text = x.BatchName, Value = x.BatchId.ToString() });
            });
            var itemnamedetails = dbcon.ItemMasters.Where(x => x.ItemCode == item_detail.ItemMasterId).FirstOrDefault();
            var itemtypeData = dbcon.ItemTypes.Where(x => x.TypeId == itemnamedetails.ItemType).FirstOrDefault();
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
            viewmodel.Expirydate = item_detail.Expirydate;
            viewmodel.PurchasePrice = item_detail.PurchasePrice;
            viewmodel.MRP = item_detail.MRP;
            viewmodel.OpeningStock = item_detail.OpeningStock;
            viewmodel.ClosingStock = item_detail.ClosingStock;
            viewmodel.ItemType = itemtypeData.TypeName;
            TempData["ItemdetailId"] = id;
            TempData.Keep();
            return View("Create", viewmodel);
        }
        [HttpPost]
        public ActionResult Edit(ItemDetail Item)
        {
            NumberFormatInfo formatprovider = new NumberFormatInfo();
            formatprovider.NumberDecimalSeparator = ",";
            formatprovider.NumberGroupSeparator = ".";
            formatprovider.NumberGroupSizes = new int[] { 2 };
            Double ItemDetailcode = Convert.ToDouble(TempData["ItemdetailId"], formatprovider);
            var itemDetailsData = dbcon.ItemDetails.Where(x => x.ItemdetailId == ItemDetailcode).FirstOrDefault();
            if (itemDetailsData != null)
            {
                if (ModelState.IsValid)
                {
                    int batchid = 0;
                    string batchname = "";
                    if (Item.BatchId != null)
                    {
                        var chk_batchname = dbcon.BatchMasters.Where(x => x.BatchId == Item.BatchId).FirstOrDefault();
                        if (chk_batchname != null)
                        {
                            batchid = Item.BatchId.HasValue ? (int)Item.BatchId : 0;
                            batchname = Item.BatchName;
                        }
                        else
                        {
                            BatchMaster bm = new BatchMaster();
                            bm.BatchName = Item.BatchName;
                            bm.Discription = "";
                            dbcon.BatchMasters.Add(bm);
                            dbcon.SaveChanges();
                            chk_batchname = dbcon.BatchMasters.Where(x => x.BatchName == Item.BatchName).FirstOrDefault();
                            {
                                batchid = chk_batchname.BatchId;
                                batchname = chk_batchname.BatchName;
                            }
                        }

                    }
                    else
                    {
                        BatchMaster bm = new BatchMaster();
                        bm.BatchName = Item.BatchName;
                        bm.Discription = "";
                        dbcon.BatchMasters.Add(bm);
                        dbcon.SaveChanges();
                        var chk_batchname = dbcon.BatchMasters.Where(x => x.BatchName == Item.BatchName).FirstOrDefault();
                        {
                            batchid = chk_batchname.BatchId;
                            batchname = chk_batchname.BatchName;
                        }
                    }

                    itemDetailsData.ItemMasterId = Item.ItemMasterId;
                    itemDetailsData.GodownId = Item.GodownId;
                    itemDetailsData.BatchId = batchid;
                    itemDetailsData.BatchName = batchname;
                    itemDetailsData.UnitId = Item.UnitId;
                    itemDetailsData.mfrdate = Item.mfrdate;
                    itemDetailsData.Expirydate = Item.Expirydate;
                    itemDetailsData.PurchasePrice = Item.PurchasePrice;
                    itemDetailsData.MRP = Item.MRP;
                    itemDetailsData.DiscPer = Item.DiscPer;
                    itemDetailsData.OpeningStock = Item.OpeningStock;
                    itemDetailsData.ClosingStock = Item.ClosingStock;
                    dbcon.Entry(itemDetailsData).State = EntityState.Modified;
                    dbcon.SaveChanges();
                    ViewBag.Message = "Item Modify Sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        public ActionResult Delete(long? id)
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
            dbcon.ItemDetails.Remove(item_detail);
            dbcon.SaveChanges();
            return RedirectToAction("Index");
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