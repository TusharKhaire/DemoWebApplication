//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoWebApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalesInvoiceMaster
    {
        public int SalesMasterId { get; set; }
        public int Accountnumber { get; set; }
        public string Accountname { get; set; }
        public int Billno { get; set; }
        public System.DateTime Invoicedate { get; set; }
        public string PaymentmodeCash { get; set; }
        public string CAddress { get; set; }
        public Nullable<long> Phoneno { get; set; }
        public string Manualno { get; set; }
        public System.DateTime Duedate { get; set; }
        public bool DontApplyGst { get; set; }
        public string CustState { get; set; }
        public Nullable<decimal> NetBillAmount { get; set; }
        public Nullable<decimal> GstAmount { get; set; }
        public Nullable<decimal> Totalbillamount { get; set; }
        public Nullable<decimal> BillDiscount { get; set; }
        public Nullable<decimal> Paidamount { get; set; }
        public Nullable<decimal> Balanceamount { get; set; }
    }
}
