using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using Eshop.Models.ViewModels;
using EshopBooks.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EshopBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }
        public IActionResult Upsert(int?id)
        {
            //to work with viewBag
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            //{
            //   Text = u.Name,
            //   Value = u.Id.ToString()
            //});
            //ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new()
            {
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
			    
                Product = new Product()
			};
            if (id==null || id==0)
            {
                //create
				return View(productVM);
			}
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
				return View(productVM);
			}
			
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file!=null)
                {
					
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageURL))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageURL = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Product");
            }
			return View(productVM);

		}
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? categoryFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        //    //Просто можно находить не только по примарному ключу как у файнда
        //    //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.DisplayOrder==id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (categoryFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(categoryFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id); if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index", "Product");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }
        #endregion
    }
}
