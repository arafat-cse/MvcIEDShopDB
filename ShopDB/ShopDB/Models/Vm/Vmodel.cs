using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDB.Models.Vm
{
    public class Vmodel
    {
        public Vmodel()
        {
            this.Products = new List<Product>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Age { get; set; }
        public Nullable<System.DateTime> UserDate { get; set; }
        public Nullable<bool> IsAudil { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public List<Product> Products { get; set; }
    }
}