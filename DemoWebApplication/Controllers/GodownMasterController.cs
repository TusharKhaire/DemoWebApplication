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
    public class GodownMasterController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: GodownMaster
        public ActionResult Index()
        {
            return View(dbcon.GodownMasters.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind] GodownMaster Gd)
        {
            ViewBag.Message = null;
            if (string.IsNullOrEmpty(Gd.GodownName))
            {
                ModelState.AddModelError("GodownName", "Plz Enter Godown Name");
                return View();
            }
            else
            {
                dbcon.GodownMasters.Add(Gd);
                dbcon.SaveChanges();
                ViewBag.Message = "Godown" + Gd.GodownName  + " saved";
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
            var godown = dbcon.GodownMasters.Find(id);
            if (godown == null)
                return HttpNotFound();
            TempData["GodownId"] = id;
            TempData.Keep();
            return View(godown);
        }
        [HttpPost]
        public ActionResult Edit([Bind]GodownMaster  godown)
        {
            if (String.IsNullOrEmpty(godown.GodownName))
            {
                ModelState.AddModelError("GodownName", "Please Enter Godown name");
                return View(godown);
            }
            NumberFormatInfo formatprovide = new NumberFormatInfo();
            formatprovide.NumberDecimalSeparator = ",";
            formatprovide.NumberGroupSeparator = ".";
            formatprovide.NumberGroupSizes = new int[] { 2 };
            Double itemtypeid = Convert.ToDouble(TempData["GodownId"], formatprovide);
            var gd = dbcon.GodownMasters .Where(x => x.GodownId  == itemtypeid).FirstOrDefault();
            if (gd != null)
            {
                gd.GodownName = godown.GodownName;
                gd.Address = godown.Address;
                dbcon.Entry(gd).State = EntityState.Modified;
                dbcon.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(godown);
        }

        [HttpPost]
        public ActionResult Delete([Bind] ItemType deleteloc)
        {
            dbcon.ItemTypes.Remove(deleteloc);
            dbcon.SaveChanges();
            return View("Index");
        }

    }
}