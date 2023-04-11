using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ShopOnline.Api.Entities;
using ShopOnline.API.Data.Repository.Contracts;
using ShopOnline.Models.dtos;

namespace ShopOnline.API.Data.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ShopOlineDbContext _db;
        public CartRepository(ShopOlineDbContext dbContext)
        {
            _db = dbContext;
        }
        private Task<bool> IsCartItemExits(int userId, int productId)
        {
            return _db.CartItems.AnyAsync(x => x.UserId == userId && x.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (cartItemToAddDto != null)
            {
                if (await IsCartItemExits(cartItemToAddDto.UserId, cartItemToAddDto.ProductId) == false)
                {
                    var item = new CartItem
                    {
                        UserId = cartItemToAddDto.UserId,
                        ProductId = cartItemToAddDto.ProductId,
                        Qty = cartItemToAddDto.Qty,
                    };
                    if (item != null)
                    {
                        var result = await _db.CartItems.AddAsync(item);
                        await _db.SaveChangesAsync();
                        return result.Entity;
                    }
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await _db.CartItems.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return item;
        }
        public async Task<IEnumerable<CartItem>> DeleteRange(IEnumerable<CartItem> cartItems)
        {
            if (cartItems != null)
            {
                _db.CartItems.RemoveRange(cartItems);
                await _db.SaveChangesAsync();
            }
            return cartItems;
        }
        public async Task<CartItem> GetItem(int id)
        {
            return await _db.CartItems.FindAsync(id);
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cartItem in _db.CartItems
                          where cartItem.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              UserId = cartItem.UserId,
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(CartItemQtyUpdateDto updateDto)
        {
            var item = await _db.CartItems.FirstOrDefaultAsync(x => x.Id == updateDto.Id);
            if (item != null)
                item.Qty = updateDto.Qty;
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
