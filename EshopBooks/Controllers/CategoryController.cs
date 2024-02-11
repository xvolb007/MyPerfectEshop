using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using EshopBooks.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace EshopBooks.Controllers
{
    public class CategoryController : Controller
    {
		private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj )
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The display order cant be same as name");
            }
            if (ModelState.IsValid)
            {
				_categoryRepo.Add(obj);
				_categoryRepo.Save();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index", "Category");
			}
                return View();
        }
		public IActionResult Edit(int? id)
		{
            if (id==null || id ==0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.GetFirstOrDefault(u=>u.Id==id);
            //Просто можно находить не только по примарному ключу как у файнда
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.DisplayOrder==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
			return View(categoryFromDb);
		}
		[HttpPost]
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)
			{
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index", "Category");
			}
			return View();
		}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
            Category? categoryFromDb = _categoryRepo.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}
		[HttpPost, ActionName("Delete")]

		public IActionResult DeletePost(int ? id)
		{
            Category? obj = _categoryRepo.GetFirstOrDefault(u => u.Id == id); if (obj == null)
			{
				return NotFound();
			}
			_categoryRepo.Remove(obj);
			_categoryRepo.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index", "Category");
		}
	}
}
