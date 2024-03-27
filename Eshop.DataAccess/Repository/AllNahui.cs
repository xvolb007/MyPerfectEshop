using Eshop.DataAccess.Repository.IRepository;
using Eshop.Models;
using EshopBooks.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository
{
    public  class AllNahui
    {
        public interface ICategoryRepository : IRepository<Category>
        {
            void Update(Category category);
        }
        public interface IProductRepository : IRepository<Product>
        {
            void Update(Product category);
        }
        public interface IRepository<T> where T : class
        {
            IEnumerable<T> GetAll(string? includeProperties = null);
            T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
            void Add(T entity);
            void Remove(T entity);
            void RemoveRange(IEnumerable<T> entities);

        }
        public interface IUnitOfWork
        {
            ICategoryRepository Category { get; }
            IProductRepository Product { get; }
            void Save();
        }
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
        public class ProductRepository : Repository<Product>, IProductRepository
        {
            private ApplicationDbContext _db;
            public ProductRepository(ApplicationDbContext db) : base(db)
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
        public class Repository<T> : IRepository<T> where T : class
        {
            private readonly ApplicationDbContext _db;
            internal DbSet<T> dbSet;


            public Repository(ApplicationDbContext db)
            {
                _db = db;
                this.dbSet = _db.Set<T>();
                _db.Products.Include(u => u.Category).Include(u => u.CategoryId);

            }

            public void Add(T entity)
            {
                dbSet.Add(entity);
            }

            public IEnumerable<T> GetAll(string? includeProperties = null)
            {
                IQueryable<T> query = dbSet;
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var property in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
                }
                return query.ToList();
            }

            public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
            {
                IQueryable<T> query = dbSet;
                query = query.Where(filter);
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var property in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
                }
                return query.FirstOrDefault();
            }

            public void Remove(T entity)
            {
                dbSet.Remove(entity);
            }

            public void RemoveRange(IEnumerable<T> entities)
            {
                dbSet.RemoveRange(entities);
            }

        }
        public class UnitOfWork : IUnitOfWork
        {
            public ICategoryRepository Category { get; private set; }
            public IProductRepository Product { get; private set; }
            private ApplicationDbContext _db;
            public UnitOfWork(ApplicationDbContext db)
            {
                _db = db;
                Category = new CategoryRepository(_db);
                Product = new ProductRepository(_db);
            }
            public void Save()
            {
                _db.SaveChanges();
            }
        }

    }
}
