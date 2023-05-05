using ShopOnline.API.Entities;
using ShopOnline.Models.dtos;
using System.Linq.Expressions;

namespace ShopOnline.Api.Data.Repository.Contracts
{
	public interface IUserRepository
	{
		Task<User> AddUser(User userDto);
		Task<User> DeleteUser(User userDto);
		//Task<User> GetUser(int id);
		//Task<User> GetUserByName(Expression<Func<User, bool>> expression,string? includeProperty = null);
		Task<User> GetUser(Expression<Func<User, bool>> expression, string? includeProperty = null);
		Task RefreshToken(string token, User user);
		Task UploadImage(string imageUrl, User user);

	}
}
