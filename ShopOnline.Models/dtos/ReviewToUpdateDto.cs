using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.dtos
{
	public class ReviewToUpdateDto
	{
		public int ReviewId { get; set; }
		public string? Content { get; set; }
		public int Rating { get; set; }
	}
}
