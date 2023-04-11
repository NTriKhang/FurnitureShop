using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Models.dtos;

namespace ShopOnline.API.Data.Repository.Contracts
{
    public interface ICartRepository
	{
		Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto);
		Task<CartItem> UpdateQty(CartItemQtyUpdateDto updateDto);
		Task<CartItem> DeleteItem(int id);
		Task<IEnumerable<CartItem>> DeleteRange(IEnumerable<CartItem> cartItems);
		Task<IEnumerable<CartItem>> GetItems(int userId);
		Task<CartItem> GetItem(int id);
	}
}
