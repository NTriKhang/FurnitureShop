using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.dtos
{
	public class UserDto
	{
		public int Id { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string? UserRole { get; set; } = string.Empty;
		public string? Token { set; get; } = string.Empty;
		public DateTime? TokenCreated { set; get; } 
		public DateTime? TokenExpires { set; get; }
	}
}
