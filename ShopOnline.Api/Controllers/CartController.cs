using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Data.Repository.Contracts;
using ShopOnline.Api.Entities;
using ShopOnline.API.Data.Repository.Contracts;
using ShopOnline.API.Extensions;
using ShopOnline.Models.dtos;
using System.Security.Claims;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartRepository cartRepository;
		private readonly IProductRepository productRepository;
		private readonly IUserRepository userRepository;
		public CartController(ICartRepository cartRepository, IProductRepository productRepository,
			IUserRepository userRepository)
		{
			this.cartRepository = cartRepository;
			this.productRepository = productRepository;
			this.userRepository = userRepository;
		}
		[HttpGet]
		[Route("/GetItemsInCart/{Token}")]
		public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(string Token)
		{
			try
			{
				var userId = await GetUserId(Token);
				var items = await cartRepository.GetItems(userId);
				if (items == null)
				{
					return NoContent();
				}
				var products = await productRepository.GetItems();
				if (products == null)
				{
					throw new Exception("No product exits in the system");
				}
				var cartItemDto = items.ConvertToDto(products);
				return Ok(cartItemDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItem(int id)
		{
			try
			{
				var items = await cartRepository.GetItem(id);
				if (items == null)
				{
					return NoContent();
				}
				var products = await productRepository.GetItem(items.ProductId);
				if (products == null)
				{
					throw new Exception("This product doesn't exits in the system");
				}
				var cartItemDto = items.ConvertToDto(products);
				return Ok(cartItemDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[NonAction]
		private async Task<int> GetUserId(string token)
		{
			try
			{
				var userId = userRepository.GetUser(x => x.Token == token).GetAwaiter().GetResult().Id;
				return userId;
			}
			catch (Exception)
			{

				throw;
			}
		}
		[HttpPost, Authorize]
		public async Task<ActionResult<IEnumerable<CartItemDto>>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
		{
			try
			{
				cartItemToAddDto.UserId = await GetUserId(cartItemToAddDto.Token);
				var newCartItem = await cartRepository.AddItem(cartItemToAddDto);
				if (newCartItem == null)
				{
					return NoContent();
				}
				var product = await productRepository.GetItem(newCartItem.ProductId);
				if (product == null)
				{
					throw new Exception("Fail to retrieve this product");
				}
				var newCartItemDto = newCartItem.ConvertToDto(product);
				return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpDelete("{id:int}")]
		public async Task<ActionResult<CartItemDto>> DeleteItemInCart(int id)
		{
			try
			{
				var itemToRemove = await cartRepository.DeleteItem(id);
				if (itemToRemove == null)
					throw new Exception($"fail to retrieve Item {id}");
				var product = await productRepository.GetItem(itemToRemove.ProductId);
				if (product == null)
					throw new Exception($"fail to retrieve product id: {itemToRemove.ProductId}");

				CartItemDto cartItemDto = itemToRemove.ConvertToDto(product);
				return Ok(cartItemDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		[HttpPatch("{id:int}")]
		public async Task<ActionResult<CartItem>> UpdateQty(CartItemQtyUpdateDto updateDto)
		{
			try
			{
				var itemToUpdate = await cartRepository.UpdateQty(updateDto);
				if(itemToUpdate == null)
					throw new Exception($"fail to retrieve Item {updateDto.Id}");
				var product = await productRepository.GetItem(itemToUpdate.ProductId);
				if (product == null)
					throw new Exception($"fail to retrieve product id: {itemToUpdate.ProductId}");

				CartItemDto cartItemDto = itemToUpdate.ConvertToDto(product);
				return Ok(cartItemDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

	}

}
