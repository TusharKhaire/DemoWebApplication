using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            foreach(var item in result)
            {
                ItemDetail id = new ItemDetail();
                var itemnamedata = dbcon.ItemMasters.Where(x => x.ItemCode == item.ItemMasterId).FirstOrDefault();
                var itemtypedata = dbcon.ItemTypes.Where(x => x.TypeId == itemnamedata.ItemType).FirstOrDefault();
                var godowndata = dbcon.GodownMasters.Where(x=>x.GodownId==item.GodownId).FirstOrDefault();
                var UnitData = dbcon.UnitMasters.Where(x=>x.UnitId==item.UnitId).FirstOrDefault();
                id.ItemdetailId = item.ItemdetailId;
                id.ItemMasterId = item.ItemMasterId;
                id.ItemName = itemnamedata.ItemName;
                id.ItemType = itemtypedata.TypeName;
                id.Godown = godowndata.GodownName;
                id.Unit = UnitData.UnitName;
                id.BatchId = item.BatchId;
                id.HsnCode = item.HsnCode;
                id.DiscPer = item.DiscPer;
                id.Expirydate = item.Expirydate;
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
            List<ItemType > itemtype = dbcon.ItemTypes.ToList();
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
            viewmodel.mfrdate =new DateTime();
            viewmodel.Expirydate =new DateTime();
            return View(viewmodel);
        }
        public JsonResult GetItemByName(string searchText)
        {
            var ItemName = dbcon.ItemMasters.Where(a => a.ItemName.Contains(searchText)).ToList();
            return Json(ItemName.Select(q => new { id = q.ItemCode, text = q.ItemName }), JsonRequestBehavior.AllowGet);
        }
    }
}