using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using DemoWebApplication.Models;

namespace DemoWebApplication.Controllers
{
    public class CustomerController : Controller  
    {
        DemoDbEntities dbconn = new DemoDbEntities();

        
        // GET: Customer
        public ActionResult Index()
        {

            return View(dbconn.AccountMasters.ToList());
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult addCustomer([Bind] AccountMaster am)
        {
            TempData["msg"] = "";
            if (ModelState.IsValid)
            {
                //string resp = dbconn.AccountMasters.Add(am);
                dbconn.AccountMasters.Add(am);
                dbconn.SaveChanges();
                TempData["msg"] = "Customer Add Succesfully";
                return View("addCustomer");
            }
            else
            {
                TempData["msg"] = "Plz Enter valid Details";
                return View();
            }
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountMaster ac = dbconn.AccountMasters.Find(id);
            if (ac == null)
            {
                return HttpNotFound();
            }
            TempData["AccountCode"] = id;
            TempData.Keep();
            return View(ac);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountMaster ac)
        {
            if (ModelState.IsValid)
            {
                NumberFormatInfo formatProvider = new NumberFormatInfo();
                formatProvider.NumberDecimalSeparator = ", ";
                formatProvider.NumberGroupSeparator = ".";
                formatProvider.NumberGroupSizes = new int[] { 2 };
                Double AcCode = Convert.ToDouble(TempData["AccountCode"], formatProvider);
                var CustomerData = dbconn.AccountMasters.Where(x => x.AccountCode == AcCode).FirstOrDefault();
                if (CustomerData != null)
                {
                    CustomerData.AccountName = ac.AccountName;
                    CustomerData.Address = ac.Address;
                    CustomerData.MobileNo = ac.MobileNo;
                    CustomerData.Email = ac.Email;
                    CustomerData.DateofBirth = ac.DateofBirth;
                    dbconn.Entry(CustomerData).State = EntityState.Modified;
                    dbconn.SaveChanges();
                }
                return RedirectToAction("Index");
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
            AccountMaster ac = dbconn.AccountMasters.Find(id);
            if (ac == null)
            {
                return HttpNotFound();
            }
            return View(ac);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            AccountMaster ac = dbconn.AccountMasters.Find(id);
            dbconn.AccountMasters.Remove(ac);
            dbconn.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}