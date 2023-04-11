using MessagePack;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShopOnline.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Api.Entities
{
    public class CartItem
    {
        [Key("Id")]
        public int Id { set; get; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { set; get; }
        //userId
        public int UserId { set; get; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { set; get; }
        public int ProductId { set; get; }
        public int Qty { set; get; }
    }
}
