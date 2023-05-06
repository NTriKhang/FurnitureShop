using Microsoft.AspNetCore.Components;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Security.Claims;

namespace ShopOnline.Solution.Pages.Product
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        private IProductServices productServices { get; set; }
        [Inject]
        private ICartItemServices cartItemServices { get; set; }
        [Inject]
        private ILocalStorageService localStorageService { get; set; }
        [Inject]
        private IUserServices userServices { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { set; get; } 

		public IEnumerable<ProductDto> ProductDtos { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ProductDtos = await productServices.GetItems();
            string Token = string.Empty;
            bool isContaint = await localStorageService.ContainKeyAsync(utility.TokenJwt);
			if (isContaint == true)
            {
                Token =  await localStorageService.GetItemAsync<string>(utility.TokenJwt) ;
                bool isUserExpires = await IsUserExpires(Token);

				if (isUserExpires == false)
                {
                    await Logout();
				}
                else
                {
					var cartItems = await cartItemServices.GetItems(Token);
					int totalQty = cartItems.Sum(x => x.Qty);
					cartItemServices.RaiseEventOnShoppingCartChanged(totalQty);
				}
            }
            else
            {
                await Logout();
            }

        }
        private async Task Logout()
        {
			await localStorageService.RemoveItemAsync("token");
			cartItemServices.RaiseEventOnShoppingCartChanged(0);
			await AuthStateProvider.GetAuthenticationStateAsync();
		}
        private async Task<bool> IsUserExpires(string Token)
        {
            var user = await userServices.GetUser(Token);
            DateTime now = DateTime.Now;
            if(user != null)
            {
                if (user.TokenCreated != null && user.TokenExpires != null)
                {
                    int cmp = DateTime.Compare(now, (DateTime)user.TokenExpires);
                    if (cmp > 0)
                        return false;
                    else
                        return true;
                }
                return false;
            }
            return true;
        }
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetProductGroupByCat()
        {
            return from product in ProductDtos
				   group product by product.CategoryId into productByCatGroup
                   orderby productByCatGroup.Key
                   select productByCatGroup;
        }
        protected string GetCatName(IGrouping<int, ProductDto> product)
        {
            return product.FirstOrDefault(x => x.CategoryId == product.Key).CategoryName;
        }
    }
}
