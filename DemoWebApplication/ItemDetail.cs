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
    
    public partial class ItemDetail
    {
        public int ItemdetailId { get; set; }
        public Nullable<int> ItemMasterId { get; set; }
        public Nullable<int> GodownId { get; set; }
        public Nullable<int> BatchId { get; set; }
        public string BatchName { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<System.DateTime> mfrdate { get; set; }
        public Nullable<System.DateTime> Expirydate { get; set; }
        public Nullable<int> PurchasePrice { get; set; }
        public Nullable<int> MRP { get; set; }
        public Nullable<int> OpeningStock { get; set; }
        public Nullable<int> ClosingStock { get; set; }
        public Nullable<int> DiscPer { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string Godown { get; set; }
        public string Unit { get; set; }
        public string HsnCode { get; set; }

    }
}
