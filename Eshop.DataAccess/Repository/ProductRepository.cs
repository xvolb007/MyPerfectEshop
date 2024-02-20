using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using EshopBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        

        public void Update(Product product)
        {
            _db.Products.Update(product);
            //var objFromDb = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Title = product.Title;
            //    objFromDb.Description = product.Description;
            //    objFromDb.Price = product.Price;
            //    objFromDb.ListPrice = product.ListPrice;
            //    objFromDb.Price100 = product.Price100;
            //    objFromDb.Price50 = product.Price50;
            //    objFromDb.ISBN = product.ISBN;
            //    objFromDb.Author = product.Author;
            //    objFromDb.CategoryId = product.CategoryId;
            //    if (product.ImageURL != null)
            //    {
            //        objFromDb.ImageURL = product.ImageURL;
            //    }

            //}

        }
    }
}
