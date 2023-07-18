﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApplication.Models;
using PagedList;
using PagedList.Mvc;

namespace DemoWebApplication.Controllers
{
    public class SalesInvoiceController : Controller
    {
        DemoDbEntities dbcon = new DemoDbEntities();
        // GET: SalesInvoice
        public ActionResult Index()
        {
            return View();
        }
        private int BillNumber()
        {
            int? billno = 0;
            billno = dbcon.SalesInvoiceMasters.Max(x => (int?)x.Billno);
            billno = billno.HasValue ? billno + 1 : 1;
            return billno ?? 1;
        }
        //NonAction method can not used as like ActionMethod.   
        //This is used for processing and functionalities purpose.  
        [NonAction]
        public SelectList ToSelectList(List<SelectList> lstState)
        {
            List<SelectListItem> stateList = new List<SelectListItem>();

            stateList.Add(new SelectListItem { Text = "Maharashtra", Value = "1" });
            stateList.Add(new SelectListItem { Text = "MP", Value = "2" });
            stateList.Add(new SelectListItem { Text = "AP", Value = "3" });
            stateList.Add(new SelectListItem { Text = "UP", Value = "4" });
            stateList.Add(new SelectListItem { Text = "Goa", Value = "5" });
            return new SelectList(stateList, "Value", "Text");
        }
        public ActionResult CreateInvoice()
        {
            ViewBag.Message = null;
            SalesInvoice viewmodel = new SalesInvoice();
            List<SelectListItem> lst_Cust = new List<SelectListItem>();
            List<AccountMaster> custNamelist = dbcon.AccountMasters.ToList();
            custNamelist.ForEach(x =>
            {
                lst_Cust.Add(new SelectListItem { Text = x.AccountName, Value = x.AccountCode.ToString() });
            });
            viewmodel.lst_CustomerNameList = lst_Cust;
            List<SelectListItem> lst_Item = new List<SelectListItem>();
            var itemnames = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).ToList();
            itemnames.ForEach(x =>
            {
                lst_Item.Add(new SelectListItem { Text = x.ItemMaster.ItemName, Value = x.ItemDetail.ItemdetailId.ToString() });
            });
            viewmodel.lst_ItemName = lst_Item;
            List<SelectList> lstState = new List<SelectList>();
            SelectList result = ToSelectList(lstState);
            viewmodel.lst_State = result.ToList();
            viewmodel.Billno = BillNumber();
            viewmodel.Invoicedate = System.DateTime.Now;
            viewmodel.Duedate = DateTime.Now;
            viewmodel.CustState = "Maharashtra";
            viewmodel.SalesMasterId = 0;
            List<string[]> salesDetailsArray = new List<string[]>();
            viewmodel.Salesdetails = salesDetailsArray;
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult CreateInvoice(int Accountnumber, String Accountname, String CAddress, long Phoneno, String Billno, DateTime Invoicedate, DateTime Duedate, int Manualno, String CustState, String Paymentmode, bool DontApplyGst, String BillDiscount, String PaidAmount, String BalanceAmount, String NetBillAmount, String GstAmount, String TotalbillAmount, int SalesMasterId, SalesInvoiceDetail[] order)
         {
            ViewBag.Message = null;
            if(SalesMasterId == 0)
            {
            if (Accountname != null && order != null)
            {
                SalesInvoiceMaster data = new SalesInvoiceMaster();
                data.Accountnumber = Accountnumber;
                data.Accountname = Accountname;
                data.CAddress = CAddress;
                if (Phoneno == 0)
                    data.Phoneno = null;
                else
                    data.Phoneno = Phoneno;
                data.Billno =Convert.ToInt32(Billno);
                data.Invoicedate = Invoicedate;
                data.Duedate = Duedate;
                if (Manualno == 0)
                    data.Manualno = null;
                else
                    data.Manualno = Convert.ToString(Manualno);
                if (CustState == "")
                    data.CustState = "Maharashtra";
                else
                    data.CustState = CustState;
                if (Paymentmode == "Cash")
                    data.PaymentmodeCash = Convert.ToString(1);
                else
                    data.PaymentmodeCash = Convert.ToString(0);
                data.DontApplyGst = DontApplyGst;
                data.BillDiscount =Convert.ToDecimal(BillDiscount);
                data.NetBillAmount =Convert.ToDecimal(NetBillAmount);
                data.GstAmount = Convert.ToDecimal(GstAmount);
                data.Totalbillamount = Convert.ToDecimal(TotalbillAmount);
                data.Paidamount = Convert.ToDecimal(PaidAmount);
                data.Balanceamount = Convert.ToDecimal(BalanceAmount); 
                dbcon.SalesInvoiceMasters.Add(data);
                dbcon.SaveChanges();

                foreach(var item in order)
                {
                    SalesInvoiceDetail id = new SalesInvoiceDetail();
                    id.Accountnumber = Accountnumber;
                    id.Billno = Convert.ToInt32(Billno);
                    var batch = dbcon.BatchMasters.Where(x => x.BatchName == item.Batchno).FirstOrDefault();
                    var godown = dbcon.GodownMasters.Where(x => x.GodownName == item.Godown).FirstOrDefault();
                    var itemdetail = dbcon.ItemDetails.Where(x => x.ItemdetailId == item.Itemdetailid).FirstOrDefault();
                    var itemmaster = dbcon.ItemMasters.Where(x => x.ItemCode == itemdetail.ItemMasterId).FirstOrDefault();
                    id.SrNo = item.SrNo;
                    id.Itemdetailid = item.Itemdetailid;
                    id.Batchno = Convert.ToString(batch.BatchId);
                    id.Godown = Convert.ToString(godown.GodownId);
                    id.Expirydate = item.Expirydate;
                    id.qty = item.qty;
                    id.salesprice = item.salesprice;
                    id.disc = item.disc;
                    id.discamt = item.discamt;
                    id.NetAmount = item.NetAmount;
                    id.totalamount = item.totalamount;
                    id.HSNCode = itemmaster.HSNCODE;
                    id.MRP = itemdetail.MRP;
                    id.PurchasePrice = itemdetail.PurchasePrice;
                    id.GST = item.GST;
                    id.IGST = item.GST;
                    if (DontApplyGst)
                    {
                        id.SrNo = 0;id.CGST = 0;
                        id.IGST = 0;
                    }
                    else
                    {
                        id.SGST = item.GST / 2;
                        id.CGST = item.GST / 2;
                    }
                    
                    id.SGSTAmt = item.SGSTAmt;
                    id.CGSTAmt = item.CGSTAmt;
                    id.IGSTAmt = item.IGSTAmt;
                    id.SGSTTaxableAmt =Convert.ToInt32( item.NetAmount - item.discamt);
                    id.CGSTTaxableAmt =Convert.ToInt32( item.NetAmount - item.discamt);
                    if (item.IGSTAmt == 0)
                    {
                        id.IGST = 0;
                        id.IGSTTaxableAmt = 0;
                    }
                    else
                    {
                        id.IGSTTaxableAmt = Convert.ToInt32(item.NetAmount - item.discamt);
                    }
                    dbcon.SalesInvoiceDetails.Add(id);
                    dbcon.SaveChanges();
                }
            }
            //ModelState.Clear();
            //ViewBag.Message = "Invoice Saved";
            return Json(new { success = true, message = "Invoice created successfully" });
                // return RedirectToAction("CreateInvoice");
            }
            else
            {
                int billno = Convert.ToInt32(Billno);
                var salesmaster = dbcon.SalesInvoiceMasters.SingleOrDefault(x => x.Billno == billno);
                var salesitemDetails = dbcon.SalesInvoiceDetails.Where(x => x.Billno == billno).FirstOrDefault();
                if (salesmaster != null && salesitemDetails != null)
                {
                    dbcon.SalesInvoiceMasters.Remove(salesmaster);
                    dbcon.SalesInvoiceDetails.Remove(salesitemDetails);
                    dbcon.SaveChanges();

                    SalesInvoiceMaster data = new SalesInvoiceMaster();
                    data.Accountnumber = Accountnumber;
                    data.Accountname = Accountname;
                    data.CAddress = CAddress;
                    if (Phoneno == 0)
                        data.Phoneno = null;
                    else
                        data.Phoneno = Phoneno;
                    data.Billno = Convert.ToInt32(Billno);
                    data.Invoicedate = Invoicedate;
                    data.Duedate = Duedate;
                    if (Manualno == 0)
                        data.Manualno = null;
                    else
                        data.Manualno = Convert.ToString(Manualno);
                    if (CustState == "")
                        data.CustState = "Maharashtra";
                    else
                        data.CustState = CustState;
                    if (Paymentmode == "Cash")
                        data.PaymentmodeCash = Convert.ToString(1);
                    else
                        data.PaymentmodeCash = Convert.ToString(0);
                    data.DontApplyGst = DontApplyGst;
                    data.BillDiscount = Convert.ToDecimal(BillDiscount);
                    data.NetBillAmount = Convert.ToDecimal(NetBillAmount);
                    data.GstAmount = Convert.ToDecimal(GstAmount);
                    data.Totalbillamount = Convert.ToDecimal(TotalbillAmount);
                    data.Paidamount = Convert.ToDecimal(PaidAmount);
                    data.Balanceamount = Convert.ToDecimal(BalanceAmount);
                    dbcon.SalesInvoiceMasters.Add(data);
                    dbcon.SaveChanges();

                    foreach (var item in order)
                    {
                        SalesInvoiceDetail id = new SalesInvoiceDetail();
                        id.Accountnumber = Accountnumber;
                        id.Billno = Convert.ToInt32(Billno);
                        var batch = dbcon.BatchMasters.Where(x => x.BatchName == item.Batchno).FirstOrDefault();
                        var godown = dbcon.GodownMasters.Where(x => x.GodownName == item.Godown).FirstOrDefault();
                        var itemdetail = dbcon.ItemDetails.Where(x => x.ItemdetailId == item.Itemdetailid).FirstOrDefault();
                        var itemmaster = dbcon.ItemMasters.Where(x => x.ItemCode == itemdetail.ItemMasterId).FirstOrDefault();
                        id.SrNo = item.SrNo;
                        id.Itemdetailid = item.Itemdetailid;
                        id.Batchno = Convert.ToString(batch.BatchId);
                        id.Godown = Convert.ToString(godown.GodownId);
                        id.Expirydate = item.Expirydate;
                        id.qty = item.qty;
                        id.salesprice = item.salesprice;
                        id.disc = item.disc;
                        id.discamt = item.discamt;
                        id.NetAmount = item.NetAmount;
                        id.totalamount = item.totalamount;
                        id.HSNCode = itemmaster.HSNCODE;
                        id.MRP = itemdetail.MRP;
                        id.PurchasePrice = itemdetail.PurchasePrice;
                        id.GST = item.GST;
                        id.IGST = item.GST;
                        if (DontApplyGst)
                        {
                            id.SrNo = 0; id.CGST = 0;
                            id.IGST = 0;
                        }
                        else
                        {
                            id.SGST = item.GST / 2;
                            id.CGST = item.GST / 2;
                        }

                        id.SGSTAmt = item.SGSTAmt;
                        id.CGSTAmt = item.CGSTAmt;
                        id.IGSTAmt = item.IGSTAmt;
                        id.SGSTTaxableAmt = Convert.ToInt32(item.NetAmount - item.discamt);
                        id.CGSTTaxableAmt = Convert.ToInt32(item.NetAmount - item.discamt);
                        if (item.IGSTAmt == 0)
                        {
                            id.IGST = 0;
                            id.IGSTTaxableAmt = 0;
                        }
                        else
                        {
                            id.IGSTTaxableAmt = Convert.ToInt32(item.NetAmount - item.discamt);
                        }
                        dbcon.SalesInvoiceDetails.Add(id);
                        dbcon.SaveChanges();
                    }
                    //return RedirectToAction("ShowAllInvoice");
                    return Json(new { success = true, message = "Invoice Modify successfully" });

                }
                else
                    return RedirectToAction("ShowAllInvoice");
            }
        }
        public ActionResult ShowAllInvoice(int?i)
        {
            var SalesData = dbcon.SalesInvoiceMasters.ToList();
            return View(SalesData.ToPagedList(i??1,5));
        }
        [HttpPost]
        public JsonResult DeleteInvoice(int id)
        {
            string msg=string.Empty;
            var salesmaster = dbcon.SalesInvoiceMasters.SingleOrDefault(x => x.Billno == id);
            var salesDetails = dbcon.SalesInvoiceDetails.SingleOrDefault(x => x.Billno == id);
            if(salesmaster !=null && salesDetails != null)
            {
                dbcon.SalesInvoiceMasters.Remove(salesmaster);
                dbcon.SalesInvoiceDetails.Remove(salesDetails);
                dbcon.SaveChanges();
                msg = "Invoice Delete successfully";
            }
            else
            {
                msg = "Invoice Not Found";
            }
            return Json(new {success=true, message = msg },JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            string msg = string.Empty;
            var salesmaster = dbcon.SalesInvoiceMasters.SingleOrDefault(x => x.Billno == id);
            var salesDetails = dbcon.SalesInvoiceDetails.Where(x => x.Billno == id).ToList();
            if (salesmaster != null && salesDetails != null)
            {
                ViewBag.Message = null;
                SalesInvoice viewmodel = new SalesInvoice();
                List<SelectListItem> lst_Cust = new List<SelectListItem>();
                List<AccountMaster> custNamelist = dbcon.AccountMasters.ToList();
                custNamelist.ForEach(x =>
                {
                    lst_Cust.Add(new SelectListItem { Text = x.AccountName, Value = x.AccountCode.ToString() });
                });
                viewmodel.lst_CustomerNameList = lst_Cust;
                List<SelectListItem> lst_Item = new List<SelectListItem>();
                var itemnames = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).ToList();
                itemnames.ForEach(x =>
                {
                    lst_Item.Add(new SelectListItem { Text = x.ItemMaster.ItemName, Value = x.ItemDetail.ItemdetailId.ToString() });
                });
                viewmodel.lst_ItemName = lst_Item;
                List<SelectList> lstState = new List<SelectList>();
                SelectList result = ToSelectList(lstState);
                viewmodel.lst_State = result.ToList();
                viewmodel.Accountname = salesmaster.Accountname;
                viewmodel.Accountnumber = salesmaster.Accountnumber;
                viewmodel.CAddress = salesmaster.CAddress;
                viewmodel.Phoneno = salesmaster.Phoneno;
                viewmodel.Billno =salesmaster.Billno;
                viewmodel.Invoicedate =salesmaster.Invoicedate;
                viewmodel.Duedate = salesmaster.Duedate;
                viewmodel.Manualno = salesmaster.Manualno;
                viewmodel.CustState = salesmaster.CustState;
                viewmodel.PaymentmodeCash = salesmaster.PaymentmodeCash;
                viewmodel.DontApplyGst = salesmaster.DontApplyGst;

                viewmodel.BillDiscount = salesmaster.BillDiscount;
                viewmodel.Paidamount = salesmaster.Paidamount;
                viewmodel.Balanceamount = salesmaster.Balanceamount;
                viewmodel.NetBillAmount = salesmaster.NetBillAmount;
                viewmodel.GstAmount = salesmaster.GstAmount;
                viewmodel.Totalbillamount = viewmodel.Totalbillamount;
                viewmodel.SalesMasterId = salesmaster.SalesMasterId;
                //viewmodel.Salesdetails = salesDetails.ToArray();
                List<string[]> salesDetailsArray = new List<string[]>();
                foreach (var details in salesDetails)
                {
                    var itemData = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Where(x => x.ItemDetail.ItemdetailId==details.Itemdetailid).SingleOrDefault();
                    int godownid = Convert.ToInt32(details.Godown);
                    var godowndata = dbcon.GodownMasters.Where(x => x.GodownId == godownid).FirstOrDefault();
                    DateTime? expiryDate = details.Expirydate;
                        string[] detailsArray = new string[] {details.SrNo.ToString(), Convert.ToString(details.Itemdetailid), itemData.ItemMaster.ItemName, itemData.ItemDetail.BatchName, godowndata.GodownName, expiryDate.ToString(),details.qty.ToString(),details.salesprice.ToString(),
                        details.disc.ToString(),details.discamt.ToString(),details.NetAmount.ToString(),details.GST.ToString(),details.IGSTAmt.ToString(),details.SGSTAmt.ToString(),details.CGSTAmt.ToString(),details.totalamount.ToString(),details.MRP.ToString(),details.PurchasePrice.ToString()};
                    salesDetailsArray.Add(detailsArray);
                }

                viewmodel.Salesdetails = salesDetailsArray;


                return View("CreateInvoice", viewmodel);
            }
            else
            {
                return HttpNotFound();
            }
            //return RedirectToAction("CreateInvoice");
            //return Json("Called",JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCustomerName(string searchText)
        {
            var Customer = dbcon.AccountMasters.Where(x => x.AccountName.Contains(searchText)).ToList();
            return Json(Customer.Select(x => new { id = x.AccountCode, text = x.AccountName }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerData(int id)
        {
            var Customerdata = dbcon.AccountMasters.Where(x => x.AccountCode == id).FirstOrDefault();
            return Json(Customerdata, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemName(string searchText)
        {
            var ItemName = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Select(i => new
            {
                ItemCode = i.ItemDetail.ItemdetailId,
                ItemName = i.ItemMaster.ItemName,
                Details = i.ItemDetail.ItemdetailId + ":" + i.ItemMaster.ItemName + ":" + i.ItemDetail.ClosingStock
            }).Where(x => x.ItemName.Contains(searchText)).ToList();
            return Json(ItemName.Select(x => new { id = x.ItemCode, text = x.Details }), JsonRequestBehavior.AllowGet);
        }
        public class itemdata
        {
            public int Itemmasterid;
            public int Itemdetailid;
            public string Itemname;
            //public int BatchId;            
            public string Batch;
            //public int GodownId;
            public string Godown;
            public string ItemType;
            //public int MfrId;
            public int mrp;
            public int purchaseprice;
            public string expdate;
            public double closingstock;
            public string HSNCOde;
            public double Gst;
        }
        public JsonResult GetItemByName(string searchText)
        {
            List<itemdata> lst_Itemdata = new List<itemdata>();
            var ItemData = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Where(x => x.ItemMaster.ItemName.Contains(searchText)).ToList();
            foreach (var item in ItemData)
            {
                itemdata obj = new itemdata();
                var batch = dbcon.BatchMasters.Where(x => x.BatchId == item.ItemDetail.BatchId).FirstOrDefault();
                var Godown = dbcon.GodownMasters.Where(x => x.GodownId == item.ItemDetail.GodownId).FirstOrDefault();
                var Type = dbcon.ItemTypes.Where(x => x.TypeId == item.ItemMaster.ItemType).FirstOrDefault();
                obj.Itemdetailid = item.ItemDetail.ItemdetailId;
                obj.Itemmasterid = item.ItemMaster.ItemCode;
                obj.Itemname = item.ItemMaster.ItemName;
                obj.ItemType = Type.TypeName;
                obj.Batch = batch.BatchName;
                obj.Godown = Godown.GodownName;
                obj.mrp = item.ItemDetail.MRP ?? 0;
                obj.purchaseprice = item.ItemDetail.PurchasePrice ?? 0;
                obj.closingstock = item.ItemDetail.ClosingStock ?? 0;
                lst_Itemdata.Add(obj);
            }

            return Json(lst_Itemdata.Select(x => new { id = x.Itemdetailid, text = x.Itemname + ":" + x.ItemType + ":" + x.Batch + ":" + x.Godown + ":" + x.closingstock }), JsonRequestBehavior.AllowGet);  // 
        }
        public JsonResult GetItemsData(int code)
        {
            var item = dbcon.ItemMasters.Join(dbcon.ItemDetails, a => a.ItemCode, b => b.ItemMasterId, (a, b) => new { ItemMaster = a, ItemDetail = b }).Where(x => x.ItemDetail.ItemdetailId == code).FirstOrDefault();

            itemdata obj = new itemdata();
            var batch = dbcon.BatchMasters.Where(x => x.BatchId == item.ItemDetail.BatchId).FirstOrDefault();
            var Godown = dbcon.GodownMasters.Where(x => x.GodownId == item.ItemDetail.GodownId).FirstOrDefault();
            var Type = dbcon.ItemTypes.Where(x => x.TypeId == item.ItemMaster.ItemType).FirstOrDefault();
            obj.Itemdetailid = item.ItemDetail.ItemdetailId;
            obj.Itemmasterid = item.ItemMaster.ItemCode;
            obj.Itemname = item.ItemMaster.ItemName;
            obj.ItemType = Type.TypeName;
            obj.Batch = batch.BatchName;
            obj.Godown = Godown.GodownName;
            obj.mrp = item.ItemDetail.MRP ?? 0;
            obj.purchaseprice = item.ItemDetail.PurchasePrice ?? 0;
            obj.closingstock = item.ItemDetail.ClosingStock ?? 0;
            obj.HSNCOde = item.ItemMaster.HSNCODE;
            obj.Gst = Convert.ToDouble(item.ItemMaster.GstPer);
            string formattedDateString = item.ItemDetail.Expirydate?.ToString("dd-MMM-yyyy");
            obj.expdate = formattedDateString;
            //obj.expdate = item.ItemDetail.Expirydate?? 0;   
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}