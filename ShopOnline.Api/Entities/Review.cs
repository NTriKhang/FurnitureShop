using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShopOnline.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Api.Entities
{
	public class Review
	{
		public int Id { get; set; }
		public string? Content { get; set; } 
		public int Rating { get; set; }
		public DateTime? Date { get; set; }
		[ForeignKey(nameof(ProductId))]
		[ValidateNever]
		public Product Product { get; set; }
		public int ProductId { get; set; }
		public string UserName { get; set; }
	}
}
