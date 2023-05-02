using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ShopOnline.Models;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ShopOnline.Solution.Services
{
	public class CartItemServices : ICartItemServices
	{
		private readonly HttpClient httpClient;
		public CartItemServices(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

        public event Action<int> OnShoppingCartChanged;
		public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
		{
			try
			{
				var response = await httpClient.PostAsJsonAsync("/api/Cart", cartItemToAddDto);
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(CartItemDto);
					}
					return await response.Content.ReadFromJsonAsync<CartItemDto>();
				}
				else
				{
					string message = await response.Content.ReadAsStringAsync();
					throw new Exception($"Http status:{response.StatusCode} Message:{message}");
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<StripeSetting> CheckOut(List<CartItemDto> cartItemDto)
		{
			try
			{
				var response = await httpClient.PostAsJsonAsync("/api/CheckoutApp/checkout", cartItemDto);
				if (response.IsSuccessStatusCode)
				{
					if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(StripeSetting);
					}
					return await response.Content.ReadFromJsonAsync<StripeSetting>();
				}
				else
				{
					string message = await response.Content.ReadAsStringAsync();
					throw new Exception($"Http status:{response.StatusCode} Message:{message}");
				}
			}
			catch (Exception)
			{

				throw;
			}

		}

		public async Task CheckOutSuccess(string UserName)
		{
			try
			{
				
				var response = await httpClient.PostAsJsonAsync("/api/CheckoutApp/success", UserName);
				if (response.IsSuccessStatusCode)
				{
					if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						throw new Exception(response.StatusCode.ToString());
					}
					return;
				}
				throw new Exception("Check out success is not occur");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<CartItemDto> DeleteItem(int id)
		{
			try
			{
				var response = await httpClient.DeleteAsync($"/api/Cart/{id}");
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(CartItemDto);
					}
					return await response.Content.ReadFromJsonAsync<CartItemDto>();
				}
				else
				{
					throw new Exception("Some thing wrong with delete Api");
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<List<CartItemDto>> GetItems(string UserName)
		{
			try
			{
				var response = await httpClient.GetAsync($"/GetItemsInCart/{UserName}");
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(List<CartItemDto>);
					}
					return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
				}
				else
				{
					string message = await response.Content.ReadAsStringAsync();
					throw new Exception(message);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
			if(OnShoppingCartChanged != null)
			{
				OnShoppingCartChanged.Invoke(totalQty);
			}
        }

        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto updateQty)
		{
			try
			{
				var jsonContent = JsonConvert.SerializeObject(updateQty);
				var content = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");
				var response = await httpClient.PatchAsync($"/api/Cart/{updateQty.Id}", content);
				if(response.IsSuccessStatusCode)
				{
					return await response.Content.ReadFromJsonAsync<CartItemDto>();
				}
				return null;
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
