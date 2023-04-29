using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.ComponentModel;

namespace ShopOnline.Solution.Pages.Cart
{
	public class CartBase : ComponentBase
	{
		[Inject]
		protected IJSRuntime Js { set; get; }

		[Inject]
		protected ICartItemServices cartItemServices { set; get; }
		[Inject]
		protected ILocalStorageService localStorageService { set; get; }
		protected List<CartItemDto> cartItems { get; set; }
		protected decimal? totalPrice { get; set; }
		protected int qty { get; set; }

		protected override async Task OnInitializedAsync()
		{
			try
			{
				var UserName = await localStorageService.GetItemAsStringAsync(utility.UserName);
				cartItems = await cartItemServices.GetItems(UserName);
                CartChange();
            }
			catch (Exception)
			{
				throw;
			}
		}
		protected async Task CheckOut(List<CartItemDto> cartItemDtos)
		{
			try
			{
				var stripeSetting = await cartItemServices.CheckOut(cartItems);
				await Js.InvokeVoidAsync("CreateCheckOut", stripeSetting.SessionId, stripeSetting.PublicKey);
			}
			catch (Exception)
			{

				throw;
			}
		}
		protected async Task DeleteButton(int id)
		{
			try
			{
				CartItemDto itemRemove = await cartItemServices.DeleteItem(id);
				RemoveCartItem(id);
                CartChange();
            }
            catch (Exception)
			{

				throw;
			}
		}
		protected async Task UpdateQty_Click(int id, int qty)
		{
			CartItemQtyUpdateDto updateDto = new CartItemQtyUpdateDto()
			{
				Id = id,
				Qty = qty
			};
			if (qty > 0)
			{
				var updateQty = await cartItemServices.UpdateQty(updateDto);
			}
			else
			{
				updateDto.Qty = 1;
				var updateQty = await cartItemServices.UpdateQty(updateDto);
			}
            CartChange();
            await Js.InvokeVoidAsync("MakeUpdateButtonVisible", id, false);
		}
		protected async Task UpdateQty_Input(int id)
		{
			await Js.InvokeVoidAsync("MakeUpdateButtonVisible", id, true);
		}
		protected void CartChange()
		{
			CalculateTheTotalPrice();
			CalculateTheQty();
			cartItemServices.RaiseEventOnShoppingCartChanged(qty);
		}
		private void CalculateTheTotalPrice()
		{
			totalPrice = cartItems.Sum(x => x.Price * x.Qty);
		}
		private void CalculateTheQty()
		{
			qty = cartItems.Sum(x => x.Qty);
		}
		private CartItemDto GetCartItem(int id)
		{
			return cartItems.FirstOrDefault(x => x.Id == id);
		}
		private void RemoveCartItem(int id)
		{
			var item = GetCartItem(id);
			cartItems.Remove(item);

        }

	}
}
