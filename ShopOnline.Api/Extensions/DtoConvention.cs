using ShopOnline.Models.dtos;
using ShopOnline.API.Entities;
using ShopOnline.API.Data.Repository;
using ShopOnline.Api.Entities;
using NuGet.Common;
using System.Runtime.CompilerServices;

namespace ShopOnline.API.Extensions
{
    public static class DtoConvention
	{
		public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> categories)
		{
			return (from product in products
					join category in categories
					on product.CategoryId equals category.Id
					select new ProductDto
					{
						Id = product.Id,
						Name = product.Name,
						Description = product.Description,
						Price = product.Price,
						ImageURL = product.ImageURL,
						Qty = product.Qty,
						CategoryId = product.CategoryId,
						CategoryName = category.Name
					}).ToList();
		}
		public static ProductDto ConvertToDto(this Product product, ProductCategory category)
		{
			return new ProductDto
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				ImageURL = product.ImageURL,
				Qty = product.Qty,
				CategoryId = product.CategoryId,
				CategoryName = category.Name,
			};
		}
		public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
		{
			return (from cartItem in cartItems
					join product in products
					on cartItem.ProductId equals product.Id
					select new CartItemDto
					{
						Id = cartItem.Id,
						ProductId = cartItem.ProductId,
						UserId = cartItem.UserId,
						ProductName = product.Name,
						ProductDescription = product.Description,
						ProductImageURL = product.ImageURL,
						Price = product.Price,
						TotalPrice = product.Price * product.Qty,
						Qty = cartItem.Qty,
					}).ToList();
		}
		public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
		{
			return new CartItemDto
			{
				Id = cartItem.Id,
				ProductId = cartItem.ProductId,
				UserId = cartItem.UserId,
				ProductName = product.Name,
				ProductDescription = product.Description,
				ProductImageURL = product.ImageURL,
				Price = product.Price,
				TotalPrice = product.Price * product.Qty,
				Qty = cartItem.Qty,
			};
		}
		public static UserDto ConvertToDto(this User user)
		{
			return new UserDto()
			{
				UserName = user.UserName,
				Id = user.Id,
				UserRole = (user.userRole?.Name == null) ? string.Empty : user.userRole.Name,
				Token = user.Token ?? null,
				TokenCreated = user.Created ?? null,
				TokenExpires = user.Expires ?? null,
			};
		}
		public static IEnumerable<ReviewDto> ConvertToDto(this IEnumerable<Review> reviews)
		{
			return (from review in reviews
					select new ReviewDto
					{
						Content = review.Content ?? string.Empty,
						UserName = review.UserName,
						DateReview = review.Date ?? DateTime.Now,
						Ratings = review.Rating,
					}).ToList();
		}
	}
}
