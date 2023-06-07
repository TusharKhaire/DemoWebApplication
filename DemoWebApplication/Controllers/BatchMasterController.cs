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
    public class BatchMasterController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: BatchMaster
        public ActionResult Index()
        {
            return View(dbcon.BatchMasters.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind] BatchMaster BM)
        {
            ViewBag.Message = null;
            if (string.IsNullOrEmpty(BM.BatchName))
            {
                ModelState.AddModelError("BatchName", "Plz Enter Batch Name");
                return View();
            }
            else
            {
                dbcon.BatchMasters .Add(BM);
                dbcon.SaveChanges();
                ViewBag.Message = "Batch" + BM.BatchName + " saved";
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
            var batch = dbcon.BatchMasters .Find(id);
            if (batch == null)
                return HttpNotFound();
            TempData["batchid"] = id;
            TempData.Keep();
            return View(batch);
        }
        [HttpPost]
        public ActionResult Edit([Bind]BatchMaster batch)
        {
            if (String.IsNullOrEmpty(batch.BatchName ))
            {
                ModelState.AddModelError("BatchName", "Please Enter Batch name");
                return View(batch);
            }
            NumberFormatInfo formatprovide = new NumberFormatInfo();
            formatprovide.NumberDecimalSeparator = ",";
            formatprovide.NumberGroupSeparator = ".";
            formatprovide.NumberGroupSizes = new int[] { 2 };
            Double itemtypeid = Convert.ToDouble(TempData["batchid"], formatprovide);
            var bm = dbcon.BatchMasters.Where(x => x.BatchId == itemtypeid).FirstOrDefault();
            if (bm != null)
            {
                bm.BatchName = batch.BatchName;
                bm.Discription = batch.Discription;
                dbcon.Entry(bm).State = EntityState.Modified;
                dbcon.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(batch);
        }

        [HttpPost]
        public ActionResult Delete([Bind] ItemType deletebatch)
        {
            dbcon.ItemTypes.Remove(deletebatch);
            dbcon.SaveChanges();
            return View("Index");
        }

    }
}