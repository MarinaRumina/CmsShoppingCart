using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare List of PageVM
            List<PageVM> pagesList;


            using (Db db = new Db())
            {
                // Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            // Return view with list
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model); // the fields that were filled out will be returned filled out so no need to fill them again
            }

            using (Db db = new Db())
            {
                // Declare slug
                string slug;

                // Init pageDTO
                PageDTO dto = new PageDTO();

                // DTO title
                dto.Title = model.Title;

                // Check for and set slug if have to be
                if (string.IsNullOrWhiteSpace(model.Slug)) // checking if empty
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                // Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "The title or slug already exists");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100; // the page that we adding will always be the last one

                // Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();

            }
            // Set TempData message (like ViewBag, just allows to save and transmit the data after redirection (unlike ViewBag))
            TempData["SuccessMessage"] = "You've added a new page!";

            // Redirect
            return RedirectToAction("AddPage"); // redirects to AddPage GET method 
        }

        // GET: Admin/Pages/EditPage/id
        public ActionResult EditPage(int id)
        {
            // Declare pageVM
            PageVM model;

            using (Db db = new Db())
            {
                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // Confirm that page exists
                if (dto == null)
                {
                    return Content("The page doesn't exist.");
                }

                // Init pageVM
                model = new PageVM(dto);

            }
            // Return view with model
            return View(model);
        }
    }
}