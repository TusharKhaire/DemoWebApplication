using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Controllers
{
    public class ItemMasterController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();

        // GET: ItemMaster
        public ActionResult Index()
        {
            return View(dbcon.ItemMasters.ToList());
        }
        //[HttpPost]
        public ActionResult AddItem([Bind] ItemMaster i)
        {
            if (ModelState.IsValid)
            {
                dbcon.ItemMasters.Add(i);
                dbcon.SaveChanges();
                return View();
            }
            else
            {
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
            if(i==null)
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
            Double itemcode = Convert.ToDouble(TempData["ItemCode"],formatProvider);
            var im = dbcon.ItemMasters.Where(x => x.ItemCode == itemcode).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (im !=null)
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

    }
}