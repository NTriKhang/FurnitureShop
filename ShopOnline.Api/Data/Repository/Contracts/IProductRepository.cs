using ShopOnline.API.Entities;

namespace ShopOnline.API.Data.Repository.Contracts
{
    public interface IProductRepository
    {
        public Task<ProductCategory> GetCategory(int id);
        public Task<IEnumerable<ProductCategory>> GetCatogories();
        public Task<Product> GetItem(int id);
        public Task<IEnumerable<Product>> GetItems();
    }
}
