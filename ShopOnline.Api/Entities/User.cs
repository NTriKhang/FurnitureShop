using MessagePack;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.API.Entities
{
    [Index(nameof(User.UserName), IsUnique =true)]
    public class User
    {
        [Key("Id")]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
		public byte[]? PasswordHash { get; set; }
		public byte[]? PasswordSalt { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(User.userRoleId))]
        public UserRole? userRole { get; set; }
        public int? userRoleId { get; set; }
        public string? Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
        public DateTime? Expires { get; set; }
    }
}
