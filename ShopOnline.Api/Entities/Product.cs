using MessagePack;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShopOnline.Api.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.API.Entities
{
    public class Product
    {
        [Key("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public int Qty { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public ProductCategory Category { get; set; }
        public int CategoryId { get; set; } 
    }
}
