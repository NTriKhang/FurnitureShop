using ShopOnline.Models;
using ShopOnline.Models.dtos;

namespace ShopOnline.Solution.Services.Contract
{
	public interface ICartItemServices
	{
		Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
		Task<List<CartItemDto>> GetItems(string UserName);
		Task<CartItemDto> DeleteItem(int id);
		Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto updateQty);
		event Action<int> OnShoppingCartChanged;
		void RaiseEventOnShoppingCartChanged(int totalQty);
		Task<StripeSetting> CheckOut(List<CartItemDto> cartItemDto);
		Task CheckOutSuccess(string UserName);
	}
}
