using ShopOnline.Api.Entities;
using ShopOnline.Models.dtos;

namespace ShopOnline.Api.Data.Repository.Contracts
{
	public interface IReviewRepository
	{
		Task<Review> AddReview(ReviewToAddDto reviewToAdd);
		Task<Review> UpdateReview(ReviewToUpdateDto reviewToUpdate);
		Task<Review> DeleteReview(int reviewId);
		Task<IEnumerable<Review>> GetReview(int productId);

	}
}
