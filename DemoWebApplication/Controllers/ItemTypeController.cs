using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Controllers
{
    public class ItemTypeController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: ItemType
        public ActionResult Index()
        {
            return View(dbcon.ItemTypes.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost ]
        public ActionResult Create([Bind] ItemType type)
        {
            ViewBag.Message = null;
            if (string.IsNullOrEmpty(type.TypeName))
            {
                ModelState.AddModelError("ItemTypeName", "Plz Enter Item Type Name");
                return View();
            }
            else
            { 
            dbcon.ItemTypes.Add(type);
            dbcon.SaveChanges();
            ViewBag.Message="Item Type "+type.TypeName +" saved";
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
            var itemtype = dbcon.ItemTypes.Find(id);
            if (itemtype == null)
                return HttpNotFound();
            TempData["TypeId"] = id;
            TempData.Keep();
            return View(itemtype);
        }
        [HttpPost]
        public ActionResult Edit([Bind]ItemType type)
        {
            if (String.IsNullOrEmpty(type.TypeName))
            {
                ModelState.AddModelError("Typename", "Please Enter Type name");
                return View(type);
            }
            NumberFormatInfo formatprovide = new NumberFormatInfo();
            formatprovide.NumberDecimalSeparator = ",";
            formatprovide.NumberGroupSeparator = ".";
            formatprovide.NumberGroupSizes = new int[] { 2 };
            Double itemtypeid = Convert.ToDouble(TempData["TypeId"],formatprovide);
            var it = dbcon.ItemTypes.Where(x => x.TypeId == itemtypeid).FirstOrDefault();
            if (it != null)
            {
                it.TypeName = type.TypeName;
                it.Details = type.Details;
                dbcon.Entry(it).State = System.Data.EntityState.Modified;
                dbcon.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(type);
        }
        
    }
}