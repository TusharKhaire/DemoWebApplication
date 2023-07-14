using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Models
{
    public class SalesInvoice
    {
        //Sales Master Data
        public int SalesMasterId { get; set; }
        public int Accountnumber { get; set; }
        public string Accountname { get; set; }
        public int Billno { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Invoicedate { get; set; }
        public string PaymentmodeCash { get; set; }
        public string CAddress { get; set; }
        public Nullable<long> Phoneno { get; set; }
        public string Manualno { get; set; }
        public System.DateTime Duedate { get; set; }
        public bool ApplyGst { get; set; }
        public string CustState { get; set; }
        public bool DontApplyGst { get; set; }
        public Nullable<decimal> NetBillAmount { get; set; }
        public Nullable<decimal> GstAmount { get; set; }
        public Nullable<decimal> Totalbillamount { get; set; }
        public Nullable<decimal> BillDiscount { get; set; }
        public Nullable<decimal> Paidamount { get; set; }
        public Nullable<decimal> Balanceamount { get; set; }

        // Sales Details Data
        public int SalesDetailId { get; set; }
        //public Nullable<int> Accountnumber { get; set; }
        //public Nullable<int> Billno { get; set; }
        public Nullable<int> Itemdetailid { get; set; }
        public string Batchno { get; set; }
        public string Godown { get; set; }
        public Nullable<System.DateTime> Expirydate { get; set; }
        public Nullable<int> qty { get; set; }
        public Nullable<int> salesprice { get; set; }
        public Nullable<int> disc { get; set; }
        public Nullable<int> discamt { get; set; }
        public Nullable<int> total { get; set; }
        public Nullable<int> SGST { get; set; }
        public Nullable<int> CGST { get; set; }
        public Nullable<int> IGST { get; set; }
        public string HSNCode { get; set; }
        public Nullable<int> SGSTAmt { get; set; }
        public Nullable<int> SGSTTaxableAmt { get; set; }
        public Nullable<int> CGSTAmt { get; set; }
        public Nullable<int> CGSTTaxableAmt { get; set; }
        public Nullable<int> SrNo { get; set; }
        public Nullable<int> MRP { get; set; }
        public Nullable<int> PurchasePrice { get; set; }
        public Nullable<int> GST { get; set; }
        public Nullable<int> IGSTAmt { get; set; }
        public Nullable<int> IGSTTaxableAmt { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public Nullable<decimal> totalamount { get; set; }

        //new Added 
        public string ItemName { get; set; }
        public IList<SelectListItem> lst_CustomerNameList { get; set; }
        public IList<SelectListItem> lst_ItemName { get; set; }
        public IList<SelectListItem> lst_State { get; set; }
        public List<SelectList> Salesdetails { get; set; }  // 14-07-2023  add bt not in use
    }
}