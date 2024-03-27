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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
