using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;

namespace ShopOnline.Solution.Pages.Review
{
	public class ReviewBase : ComponentBase
	{
		[Parameter]
		public int ProductId { get; set; }
		[Inject]
		private IReviewServices reviewServices { set; get; }
		[Inject]
		private IJSRuntime Js { set; get; }
		[Inject]
		private NavigationManager navigationManager { set; get; }

		protected IEnumerable<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
		protected ReviewToAddDto reviewToAddDto { get; set; } = new ReviewToAddDto();
		protected override async Task OnInitializedAsync()
		{
			Reviews = await reviewServices.GetReviewDto(productId: ProductId);
		}
		protected async Task Onclick_submit()
		{
			try
			{
				if(reviewToAddDto == null)
					throw new ArgumentNullException(nameof(reviewToAddDto));
				reviewToAddDto.ProductId = ProductId;
				var userReview = await reviewServices.PostReviewDto(reviewToAddDto);
			}
			catch (Exception)
			{

				throw;
			}
		}
		protected async Task Onclick_star(int value)
		{
			await Js.InvokeVoidAsync("ChangeStarColor", value);
			reviewToAddDto.Rating = value;
		}
	}
}
