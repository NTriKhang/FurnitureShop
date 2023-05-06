using Microsoft.AspNetCore.Components;
using ShopOnline.Solution.Services;
using ShopOnline.Solution.Services.Contract;

namespace ShopOnline.Solution.Pages.Cart
{
	public class SuccessBase : ComponentBase
	{
		[Inject]
		protected ICartItemServices cartItemServices { set; get; }
		[Inject]
		private ILocalStorageService localStorageService { set; get; }
		public string Token { set; get; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
			try
			{
				Token = await localStorageService.GetItemAsync<string>(utility.TokenJwt);
				await cartItemServices.CheckOutSuccess(Token);
			}
			catch (Exception)
			{

				throw;
			}
        }

    }
}
