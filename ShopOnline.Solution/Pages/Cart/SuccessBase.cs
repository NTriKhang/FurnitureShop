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
		public string UserName { set; get; }

        protected override async Task OnInitializedAsync()
        {
			try
			{
				UserName = await localStorageService.GetItemAsStringAsync("UserName");
				await cartItemServices.CheckOutSuccess(UserName);
			}
			catch (Exception)
			{

				throw;
			}
        }

    }
}
