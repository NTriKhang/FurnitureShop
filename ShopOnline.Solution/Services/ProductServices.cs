using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Net.Http.Json;

namespace ShopOnline.Solution.Services
{
    public class ProductServices : IProductServices
    {
        private readonly HttpClient httpClient;
        public ProductServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

		public async Task<ProductDto> GetItem(int id)
		{
            try
            {
                var response = await httpClient.GetAsync($"/api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }
                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
		}

		public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var response = await httpClient.GetAsync("/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>(); 
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
