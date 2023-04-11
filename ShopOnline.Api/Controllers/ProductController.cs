using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Data.Repository.Contracts;
using ShopOnline.API.Extensions;


namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                var product = await _productRepository.GetItem(id);
                if(product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var category = await _productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(category);
                    return Ok(productDto);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var products = await _productRepository.GetItems();
                var categories = await _productRepository.GetCatogories();
                if(products == null || categories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productsDto = products.ConvertToDto(categories);
                    return Ok(productsDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }
    }
}
