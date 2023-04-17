using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Metadata;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services;
using ShopOnline.Solution.Services.Contract;
using System.Security.Claims;

namespace ShopOnline.Solution.Pages.Login
{
	public class LoginBase : ComponentBase
	{
		[Inject]
		protected IUserServices _userServices { set; get; }
		[Inject]
		protected NavigationManager _navigationManager { set; get; }
		[Inject]
		protected AuthenticationStateProvider _authorizeState { set; get; }
		[Inject]
		protected ILocalStorageService _localStorageService { set; get; }

		public UserLoginDto userLoginDto { get; set; } = new UserLoginDto();

		protected override void OnInitialized()
		{
			try
			{
				
			}
			catch (Exception)
			{

				throw;
			}
		}
		protected async Task Login_OnClick()
		{
			try
			{
				var token = await _userServices.LoginUser(userLoginDto);
				Console.WriteLine(token);
				await _localStorageService.SetItemAsStringAsync(utility.UserName, userLoginDto.UserName);
				await _localStorageService.SetItemAsync(utility.TokenJwt, token);
				await _authorizeState.GetAuthenticationStateAsync();
				_navigationManager.NavigateTo("/");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
