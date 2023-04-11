using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Data.Repository.Contracts;
using ShopOnline.Api.Entities;
using ShopOnline.API.Extensions;
using ShopOnline.Models.dtos;

namespace ShopOnline.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : ControllerBase
	{
		private readonly IReviewRepository reviewRepository;

		public ReviewController(IReviewRepository reviewRepository, IUserRepository userRepository)
		{
			this.reviewRepository = reviewRepository;
		}
		[HttpPost("SubmitReview")]
		[Authorize]
		public async Task<IActionResult> AddReview([FromBody] ReviewToAddDto reviewToAdd)
		{
			try
			{
				reviewToAdd.UserName = User.Identity?.Name ?? "Not found";
				var newReview = await reviewRepository.AddReview(reviewToAdd);
				if (newReview == null)
					throw new Exception("Add review is not work");
				return CreatedAtAction(nameof(AddReview), newReview);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		[HttpGet("{ProductId}", Name = nameof(GetReviews))]
		public async Task<IActionResult> GetReviews(int ProductId)
		{
			try
			{
				if(ProductId == 0)
					return BadRequest(string.Empty);
				var Reviews = await reviewRepository.GetReview(ProductId);
				var res = Reviews.ConvertToDto();
				return Ok(res);
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
