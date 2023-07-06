using System;
using System.Collections.Generic;
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
        public System.DateTime Invoicedate { get; set; }
        public string PaymentmodeCash { get; set; }
        public Nullable<int> Totalbillamount { get; set; }
        public Nullable<int> Paidamount { get; set; }
        public Nullable<int> Balanceamount { get; set; }
        public string CAddress { get; set; }
        public Nullable<long> Phoneno { get; set; }
        public string Manualno { get; set; }
        public Nullable<int> BillDiscount { get; set; }
        public System.DateTime Duedate { get; set; }
        public bool ApplyGst { get; set; }
        public string CustState { get; set; }

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

        //new Added 
        public string ItemName { get; set; }
        public Double NetAmout { get; set; }
        public Double GstAmount { get; set; }
        public IList<SelectListItem> lst_CustomerNameList { get; set; }
        public IList<SelectListItem> lst_ItemName { get; set; }
        public IList<SelectListItem> lst_State { get; set; }
    }
}