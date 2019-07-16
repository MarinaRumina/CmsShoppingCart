using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Models.ViewModels
{
    public class SidebarVM
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }

        public SidebarVM(SidebarDTO row)
        {
            this.Id = row.Id;
            this.Body = row.Body;
        }
        public SidebarVM()
        {

        }
    }
}