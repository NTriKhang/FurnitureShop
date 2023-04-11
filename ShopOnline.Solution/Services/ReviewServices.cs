using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Net.Http;
using System.Net.Http.Json;

namespace ShopOnline.Solution.Services
{
	public class ReviewServices : IReviewServices
	{
		private readonly HttpClient httpClient;
		public ReviewServices(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<IEnumerable<ReviewDto>> GetReviewDto(int productId)
		{
			try
			{
				var response = await httpClient.GetAsync($"/api/Review/{productId}");
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(IEnumerable<ReviewDto>);
					}
					return await response.Content.ReadFromJsonAsync<IEnumerable<ReviewDto>>();
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

		public async Task<ReviewDto> PostReviewDto(ReviewToAddDto reviewToAddDto)
		{
			try
			{
				var response = await httpClient.PostAsJsonAsync("/api/Review/SubmitReview", reviewToAddDto);
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(ReviewDto);
					}
					
					return await response.Content.ReadFromJsonAsync<ReviewDto>();
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
	}
}
