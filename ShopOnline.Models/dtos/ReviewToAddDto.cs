using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.dtos
{
	public class ReviewToAddDto
	{
		public string UserName { get; set; } = string.Empty;
		public int ProductId { get; set; }
		public string? Content { get; set; }
		public int Rating { get; set; }
	}
}
