using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data.Repository.Contracts;
using ShopOnline.Api.Entities;
using ShopOnline.API.Data;
using ShopOnline.Models.dtos;

namespace ShopOnline.Api.Data.Repository
{
	public class ReviewRepository : IReviewRepository
	{
		private readonly ShopOlineDbContext _context;
        public ReviewRepository(ShopOlineDbContext context)
        {
            _context = context;
        }
		public async Task<IEnumerable<Review>> GetReview(int productId)
		{
			try
			{
				IEnumerable<Review> reviews = await _context.Reviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.Date).ToListAsync();
				return reviews;		
			}
			catch (Exception)
			{
				throw;
			}
		}
		private async Task<bool> isUserAlreadyReview(string Username, int productId)
		{
			return await _context.Reviews.AnyAsync(x => x.UserName == Username && x.ProductId == productId);
		}
        public async Task<Review> AddReview(ReviewToAddDto reviewToAdd)
		{
			try
			{
				if (reviewToAdd != null)
				{
					if (!isUserAlreadyReview(reviewToAdd.UserName, reviewToAdd.ProductId).GetAwaiter().GetResult())
					{
						var item = new Review
						{
							UserName = reviewToAdd.UserName,
							ProductId = reviewToAdd.ProductId,
							Content = reviewToAdd.Content,
							Rating = reviewToAdd.Rating,
							Date = DateTime.Now,
						};
						if (item != null)
						{
							var result = await _context.Reviews.AddAsync(item);
							await _context.SaveChangesAsync();
							return result.Entity;
						}
					}
					else
					{
						throw new Exception("This user has already made this product review");
					}
				}
				throw new Exception("This review is not valid");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public Task<Review> DeleteReview(int reviewId)
		{
			throw new NotImplementedException();
		}

		public Task<Review> UpdateReview(ReviewToUpdateDto reviewToUpdate)
		{
			throw new NotImplementedException();
		}
	}
}
