using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace DemoWebApplication.Models
{
    public class ItemDetailM
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

        public IList<SelectListItem> itemDetailList { get; set; }
        public IList<SelectListItem> lstitemtypes { get; set; }
        //public IList<SelectListItem> lstitemgroups { get; set; }
        public IList<SelectListItem> lstunits { get; set; }
        public IList<SelectListItem> lstgodowns { get; set; }
        public IList<SelectListItem> lstbatches { get; set; }
    }
}