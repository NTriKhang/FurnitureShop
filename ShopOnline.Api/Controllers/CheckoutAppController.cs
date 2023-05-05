using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Data.Repository.Contracts;
using ShopOnline.Api.Entities;
using ShopOnline.API.Data.Repository.Contracts;
using ShopOnline.API.Entities;
using ShopOnline.API.Extensions;
using ShopOnline.Models;
using ShopOnline.Models.dtos;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace ShopOnline.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CheckoutAppController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IServiceProvider _serviceProvider;
		private readonly ICartRepository _cartRepository;
		private readonly IProductRepository _productRepository;
		private readonly IUserRepository _userRepository;

		public CheckoutAppController(IConfiguration configuration, IServiceProvider serviceProvider,
			ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository)
		{
			_configuration = configuration;
			_serviceProvider = serviceProvider;
			_cartRepository = cartRepository;
			_productRepository = productRepository;
			_userRepository = userRepository;
		}
		string thisUrl = string.Empty;

		[HttpPost("checkout")]
		public async Task<ActionResult<StripeSetting>> CreateCheckout([FromBody] List<CartItemDto> cartItems)
		{

			var server = _serviceProvider.GetRequiredService<IServer>();
			var serverAddressFuture = server.Features.Get<IServerAddressesFeature>();

			if (serverAddressFuture is not null)
			{
				thisUrl = serverAddressFuture.Addresses.FirstOrDefault();
			}
			if (thisUrl is not null)
			{
				var pubKey = _configuration.GetSection("StripeSetting:PublicKey").Get<string>();
				var sessionId = await Create(cartItems);

				var StripeSetting = new StripeSetting
				{
					PublicKey = pubKey,
					SessionId = sessionId
				};

				return Ok(StripeSetting);
			}
			return StatusCode(505);
		}
		[NonAction]
		public async Task<string> Create(List<CartItemDto> cartItems)
		{
			var urlrequest = Request.Headers.Referer[0];
			var options = new SessionCreateOptions
			{
				Mode = "payment",
				SuccessUrl = urlrequest + "success",
				CancelUrl = urlrequest + "cancel",
				LineItems = new List<SessionLineItemOptions>(),
			};
			foreach (var cartItem in cartItems)
			{
				SessionLineItemOptions LineItems = new SessionLineItemOptions()
				{
					// Provide the exact Price ID (for example, pr_1234) of the product you want to sell
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmountDecimal = cartItem.Price * 1000,
						Currency = "vnd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = cartItem.ProductName,

						},
					},
					Quantity = cartItem.Qty,
				};
				options.LineItems.Add(LineItems);
			}
			var service = new SessionService();
			Session session = await service.CreateAsync(options);
			
			return session.Id;
		}
		[HttpPost("success")]
		public async Task<IEnumerable<CartItemDto>> CheckoutSuccess([FromBody]string Token)
		{
			try
			{
				var userId = _userRepository.GetUser(x => x.Token == Token).GetAwaiter().GetResult().Id;
				if(userId == 0)
                    throw new Exception($"userId is null");
                var cartItem = await _cartRepository.GetItems(userId);
				if (cartItem == null)
					throw new Exception($"Can't retrive item within this {userId}");
                var cartItemDelete = await _cartRepository.DeleteRange(cartItem);
				if (cartItemDelete == null)
					throw new Exception("Delete cart items were false");
				var products = await _productRepository.GetItems();
				var cartItemDto = cartItemDelete.ConvertToDto(products);
				return cartItemDto;
            }
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
