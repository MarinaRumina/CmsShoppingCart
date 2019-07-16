using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM(PageDTO row)
        {
            this.Id = row.Id;
            this.Title = row.Title;
            this.Slug = row.Slug;
            this.Body = row.Body;
            this.Sorting = row.Sorting;
            this.HasSidebar = row.HasSidebar;
        }

        public PageVM() { }

        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        [Display(Name = "Sidebar")]
        public bool HasSidebar { get; set; }
    }
}