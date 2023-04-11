using ShopOnline.Models.dtos;

namespace ShopOnline.Solution.Services.Contract
{
	public interface IReviewServices
	{
		Task<IEnumerable<ReviewDto>> GetReviewDto(int productId);
		Task<ReviewDto> PostReviewDto(ReviewToAddDto reviewToAddDto);
	}
}
