using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShopOnline.API.Data;
using ShopOnline.API.Entities;
using ShopOnline.Models.dtos;
using Stripe.Issuing;
using System.Linq.Expressions;
using System.Reflection;

namespace ShopOnline.Api.Data.Repository.Contracts
{
	public class UserRepository : IUserRepository
	{
		private readonly ShopOlineDbContext _dbContext;
        public UserRepository(ShopOlineDbContext dbContext)
        {
			_dbContext = dbContext;
        }

		public async Task<User> GetUserByName(Expression<Func<User, bool>> expression, string? includeProperty = null)
		{
			IQueryable<User> query = _dbContext.Users.Where(expression);
			if (query.Count() < 0)
				return null;
			if(includeProperty != null)
			{
				foreach(var includeProp in includeProperty.Split(","))
				{
					query = query.Include(includeProp.Trim());
				}
			}
			return await query.FirstOrDefaultAsync();
		}
        public async Task<User> AddUser(User userToAdd)
		{
			if(userToAdd != null)
			{
				var user = await _dbContext.Users.AddAsync(userToAdd);
				await _dbContext.SaveChangesAsync();
				return user.Entity;
			}
			return null;
		}

		public Task<User> DeleteUser(User userToAdd)
		{
			throw new NotImplementedException();
		}

		public async Task<User> GetUser(int id)
		{
			if(id != 0)
			{
				var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
				if (user != null)
					return user;
			}
			return null;
		}

		public async Task RefreshToken(string token, User user)
		{
			user.Token = token;
			user.Created = DateTime.Now;
			user.Expires = DateTime.Now.AddDays(1);
			await _dbContext.SaveChangesAsync();
		}
	}
}
