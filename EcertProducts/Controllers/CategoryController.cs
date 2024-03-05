using BusinessLogic.Models;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EcertProducts.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {

        private ICategoryService _service;


        public CategoryController(ICategoryService categoryService)
        {
            _service = categoryService;
        }



        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var data =  _service.GetAllCategorys();
            return View(data);
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var categoryDetails = await _service.GetCategoryById(id);

            if (categoryDetails == null)
                return View("Not Found");

            return View(categoryDetails);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                await _service.CreateCategory(model);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(ex);
            }
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var categoryDetails = await _service.GetCategoryById(id);

            if (categoryDetails == null)
                return View("Not Found");

            return View(categoryDetails);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryModel model)
        {
            try
            {
                CategoryModel category = new CategoryModel();
                category = model;
                 await _service.UpdateCategory(category);               
            }
            catch
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var categoryDetails = await _service.GetCategoryById(id);

            if (categoryDetails == null)
                return View("Not Found");

            return View(categoryDetails);

        }

        // POST: CategoryController/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {

             await   _service.DeleteCategory(id);
                   
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
