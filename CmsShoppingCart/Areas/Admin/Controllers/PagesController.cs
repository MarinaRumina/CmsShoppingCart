using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels;
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
        [HttpGet]
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

        // GET: Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            using (Db db = new Db())
            {
                // Get page Id
                int id = model.Id;

                // Declare slug
                string slug = "home";

                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // DTO the title
                dto.Title = model.Title;

                // Check the slug and set it if needed
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                // Make sure that the title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title)
                    || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "This title or slug is already exist.");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                // Save the DTO
                db.SaveChanges();

            }

            // Set TempData message
            TempData["SuccessMessage"] = "You're changes have been saved.";

            // Redirect
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            // Declare PageVM
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

        // DELETE: Admin/Page/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                // Get the page
                PageDTO dto = db.Pages.Find(id);

                // Remove the page
                db.Pages.Remove(dto);

                // Save
                db.SaveChanges();

            }
            // Redirect
            return RedirectToAction("Index");
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                // Set initial count
                int count = 1; // HomePage Sorting order is 0, so all other pages will be after the Home page

                // Declare PageDTO
                PageDTO dto;

                // Set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }

        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            // Declare the model
            SidebarVM model;

            using (Db db = new Db())
            {
                // Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);    // we have only one sidebar

                // Init the model
                model = new SidebarVM(dto);
            }

            // Return view with the model
            return View(model);
        }

        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult Editsidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                // Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // DTO the body
                dto.Body = model.Body;

                // Save
                db.SaveChanges();
            }
            // Set TempData message
            TempData["SuccessMessage"] = "Your changes to the sidebar were saved successfully";

            // Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}