using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsApp.Models
{
    public class Repository
    {
        private static readonly List<Product> _products = new();
        private static readonly List<Category> _categories = new();

        static Repository()
        {
            _categories.Add(new Category(){Id=1,Name="Phone"});
            _categories.Add(new Category(){Id=2,Name="Computer"});
            _products.Add(new Product(){Id=1,Name="Iphone 12",Price=20000,IsActive=true,Image="1.jpg",CategoryId=1});
            _products.Add(new Product(){Id=2,Name="Iphone 13",Price=30000,IsActive=true,Image="2.jpg",CategoryId=1});
            _products.Add(new Product(){Id=3,Name="Iphone 14",Price=40000,IsActive=true,Image="3.jpg",CategoryId=1});
            _products.Add(new Product(){Id=4,Name="Iphone 15",Price=50000,IsActive=true,Image="4.jpg",CategoryId=1});
            _products.Add(new Product(){Id=5,Name="Macbook Air",Price=60000,IsActive=true,Image="5.jpg",CategoryId=2});
            _products.Add(new Product(){Id=6,Name="Macbook Pro",Price=70000,IsActive=true,Image="6.jpg",CategoryId=2});
        }

        public static List<Product> Products
        {
            get{
                return _products;
            }
        }

        public static void Create(Product entity)
        {
            _products.Add(entity);
        }
        public static void Edit(Product updatedProduct)
        {
            var entity = _products.FirstOrDefault(p => p.Id==updatedProduct.Id);
            if(entity != null)
            {
                entity.Name = updatedProduct.Name;
                entity.Price = updatedProduct.Price;
                entity.Image = updatedProduct.Image;
                entity.CategoryId = updatedProduct.CategoryId;
            }
        }

        public static void Delete (Product product)
        {
            var entity = _products.FirstOrDefault(p => p.Id==product.Id);
            if(entity != null)
            {
                _products.Remove(entity);
            }
        }

        public static List<Category> Categories
        {
            get{
                return _categories;
            }
        }
    }
}