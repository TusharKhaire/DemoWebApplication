using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoWebApplication.Models
{
    public class ItemMasterM
    {
        [Required]
        public int ItemCode { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public int ItemType { get; set; }
        public string HSNCODE { get; set; }
        public string GstPer { get; set; }
        public string TypeName { get; set; }
        public IList<SelectListItem> itemtypes { get; set; }
    }
}