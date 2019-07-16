using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.ViewModels.Shop
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }

        public CategoryVM(CategoryDTO row)
        {
            this.Id = row.Id;
            this.Name = row.Name;
            this.Slug = row.Slug;
            this.Sorting = row.Sorting;
        }

        public CategoryVM()
        {

        }
    }
}