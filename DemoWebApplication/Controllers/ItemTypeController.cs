using System;
using System.Collections.Generic;
using System.Linq;
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
        
    }
}