using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Services;
using DemoWebApplication.Enums;
using System.Data.Entity;
namespace DemoWebApplication.Controllers
{
    public class ItemMasterController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();

        // GET: ItemMaster
        public ActionResult Index()
        {
            List<ItemMaster> items = new List<ItemMaster>();
            var result = dbcon.ItemMasters.ToList();
            foreach(var item in result )
            {
                ItemMaster im = new ItemMaster();
                var itemTypedata = dbcon.ItemTypes.Where(x => x.TypeId == item.ItemType).FirstOrDefault();
                im.TypeName =itemTypedata.TypeName ;
                im.ItemCode = item.ItemCode;
                im.ItemName = item.ItemName;
                im.ItemType = item.ItemType;
                im.HSNCODE = item.HSNCODE;
                im.GstPer = item.GstPer;
                items.Add(im);
            }
            return View(items);
        }
        [HttpGet]
        public ActionResult AddItem()
        {
            ItemMaster viewmodel = new ItemMaster();
            List<SelectListItem> itemtypename = new List<SelectListItem>();
            List<ItemType> typelist = dbcon.ItemTypes.ToList();
            foreach(var type in typelist)
            {
                itemtypename.Add(new SelectListItem { Text = type.TypeName, Value = type.TypeId.ToString() });
            }
            viewmodel.itemtypes = itemtypename;
            return View(viewmodel );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem([Bind] ItemMaster i)
        {
            ViewBag.Message = string.Format("");
            if (ModelState.IsValid)
            {
                var im = dbcon.ItemMasters.Where(x => x.ItemName == i.ItemName && x.ItemType == i.ItemType).FirstOrDefault();
                if (im == null)
                {
                    dbcon.ItemMasters.Add(i);
                    dbcon.SaveChanges();
                   // ViewBag.Message = String.Format("Item Name " + i.ItemName + " save succesfully");
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Alert.Success, "Item Name " + i.ItemName + " save succesfully");  //String.Format("Item Name " + i.ItemName + " save succesfully");
                    return View();
                }
                else
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Alert.Warning, "Item Name " + i.ItemName + " With Item type " + i.ItemType + " already exist");
                   // ViewBag.Message = String.Format("Item Name "+ i.ItemName +" With Item type "+ i.ItemType + "already exist");
                    return View(i);
                }
               
            }
            else
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Alert.Danger, "Please Enter Valid Details");
                //ViewBag.Message = String.Format("Please Enter Valid Details");
                return View(i);
            }
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemMaster i = dbcon.ItemMasters.Find(id);
            if (i == null)
            {
                return HttpNotFound();
            }
            TempData["ItemCode"] = id;
            TempData.Keep();
            return View(i);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemMaster i)
        {
            NumberFormatInfo formatProvider = new NumberFormatInfo();
            formatProvider.NumberDecimalSeparator = ", ";
            formatProvider.NumberGroupSeparator = ".";
            formatProvider.NumberGroupSizes = new int[] { 2 };
            Double itemcode = Convert.ToDouble(TempData["ItemCode"], formatProvider);
            var im = dbcon.ItemMasters.Where(x => x.ItemCode == itemcode).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (im != null)
                {
                    im.ItemName = i.ItemName;
                    im.ItemType = i.ItemType;
                    im.HSNCODE = i.HSNCODE;
                    im.GstPer = i.GstPer;
                    dbcon.Entry(im).State = EntityState.Modified;
                    dbcon.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Edit");
            }
            else
                return RedirectToAction("Edit");
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemMaster im = dbcon.ItemMasters.Find(id);
            if (im == null)
            {
                return HttpNotFound();
            }
            return View(im);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(long id)
        {
            ItemMaster im = dbcon.ItemMasters.Find(id);
            dbcon.ItemMasters.Remove(im);
            dbcon.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}