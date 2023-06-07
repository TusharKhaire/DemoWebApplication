using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Controllers
{
    public class UnitMasterController : Controller
    {
        // GET: UnitMaster
        DemoDbEntities dbcon = new DemoDbEntities();
        public ActionResult Index()
        {
            return View(dbcon.UnitMasters.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind] UnitMaster um)
        {
            ViewBag.Message = null;
            if (string.IsNullOrEmpty(um.UnitName))
            {
                ModelState.AddModelError("UnitName", "Plz Enter Unit Name");
                return View();
            }
            else
            {
                dbcon.UnitMasters.Add(um);
                dbcon.SaveChanges();
                ViewBag.Message = "Unit " + um.UnitName + " saved";
                ModelState.Clear();
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var unit = dbcon.UnitMasters.Find(id);
            if (unit == null)
                return HttpNotFound();
            TempData["unitid"] = id;
            TempData.Keep();
            return View(unit);
        }
        [HttpPost]
        public ActionResult Edit([Bind]UnitMaster unit)
        {
            if (String.IsNullOrEmpty(unit.UnitName))
            {
                ModelState.AddModelError("UnitName", "Please Enter Unit name");
                return View(unit);
            }
            NumberFormatInfo formatprovide = new NumberFormatInfo();
            formatprovide.NumberDecimalSeparator = ",";
            formatprovide.NumberGroupSeparator = ".";
            formatprovide.NumberGroupSizes = new int[] { 2 };
            Double itemtypeid = Convert.ToDouble(TempData["unitid"], formatprovide);
            var bm = dbcon.UnitMasters.Where(x => x.UnitId == itemtypeid).FirstOrDefault();
            if (bm != null)
            {
                bm.UnitName = unit.UnitName;
                dbcon.Entry(bm).State = EntityState.Modified;
                dbcon.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(unit);
        }

        [HttpPost]
        public ActionResult Delete([Bind] ItemType deleteunit)
        {
            dbcon.ItemTypes.Remove(deleteunit);
            dbcon.SaveChanges();
            return View("Index");
        }
    }
}