using Microsoft.EntityFrameworkCore;
using ShopOnline.API.Data;
using ShopOnline.API.Data.Repository.Contracts;
using ShopOnline.API.Entities;
using System.Reflection.Metadata.Ecma335;

namespace ShopOnline.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOlineDbContext _shopOlineDbContext;
        public ProductRepository(ShopOlineDbContext shopOlineDbContext)
        {
            _shopOlineDbContext = shopOlineDbContext;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _shopOlineDbContext.ProductCategories.FindAsync(id);
            return category;
        }

        public async Task<IEnumerable<ProductCategory>> GetCatogories()
        {
            var categories = await _shopOlineDbContext.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<Product> GetItem(int id)
        {
            var product = await _shopOlineDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await _shopOlineDbContext.Products.ToListAsync();
            return products;
        }
    }
}
