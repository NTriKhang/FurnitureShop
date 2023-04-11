using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Security.Claims;

namespace ShopOnline.Solution.Pages.ProductDetail
{
    public class ProductDetailBase : ComponentBase
    {
        [Parameter]
        public int Id { set; get; }
		[Inject]
        private IProductServices productServices { get; set; }
		[Inject]
		private ICartItemServices cartItemServices { get; set; }
		[Inject]
		private NavigationManager navigationManager { get; set; }

        public ProductDto product { set; get; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				product = await productServices.GetItem(Id);

			}
			catch (Exception)
			{
				throw;
			}
		}
		protected async Task AddToCart_Button(CartItemToAddDto cartItemToAddDto)
		{
			try
			{
				var cartItemDto = await cartItemServices.AddItem(cartItemToAddDto);
				navigationManager.NavigateTo("/Cart");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
