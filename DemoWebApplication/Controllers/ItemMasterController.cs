using System;
using System.Collections.Generic;
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
            TempData["ItemCode"] = i;
            TempData.Keep();
            return View(i);
        }
    }
}