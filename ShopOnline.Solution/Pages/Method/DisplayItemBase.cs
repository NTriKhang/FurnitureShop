using Microsoft.AspNetCore.Components;
using ShopOnline.Models.dtos;

namespace ShopOnline.Solution.Pages.Method
{
	public class DisplayItemBase : ComponentBase
	{
		[Parameter]
		public IEnumerable<ProductDto> products { get; set; }
	}
}
