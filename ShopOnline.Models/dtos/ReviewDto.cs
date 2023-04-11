using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.dtos
{
	public class ReviewDto
	{
		public string UserName { get; set; } = string.Empty;
		public int Ratings { get; set; }
		public string Content { get; set; } = string.Empty;
		public DateTime DateReview { get; set; }

	}
}
