using ShopOnline.Models.dtos;

namespace ShopOnline.Solution.Services.Contract
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetItem(int id);
    }
}
