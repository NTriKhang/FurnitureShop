using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Net.Http;
using System.Net.Http.Json;

namespace ShopOnline.Solution.Services
{
	public class UserServices : IUserServices
	{
		private readonly HttpClient httpClient;
		public UserServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
		public async Task<UserDto> GetUserByName(string UserName)
		{
			try
			{
				var response = await httpClient.GetAsync($"/api/UserManagement/{UserName}");
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
						return default(UserDto);
					return await response.Content.ReadFromJsonAsync<UserDto>();
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
		public async Task<UserDto> RegisterUser(UserRegisterDto userRegisterDto)
		{
			try
			{
				var response = await httpClient.PostAsJsonAsync("/api/UserManagement/register", userRegisterDto);
				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
						return default(UserDto);
					return await response.Content.ReadFromJsonAsync<UserDto>();
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

		public async Task<string> LoginUser(UserLoginDto userLoginDto)
		{
			try
			{
				var response = await httpClient.PostAsJsonAsync("/api/UserManagement/login", userLoginDto);
				if(response.IsSuccessStatusCode)
				{
					if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
						return default(string);
					return await response.Content.ReadAsStringAsync();
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
