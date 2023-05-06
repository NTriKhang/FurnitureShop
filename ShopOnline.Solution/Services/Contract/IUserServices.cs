using ShopOnline.Models.dtos;

namespace ShopOnline.Solution.Services.Contract
{
	public interface IUserServices
	{
		Task<string> LoginUser(UserLoginDto userLoginDto);
		Task<UserDto> RegisterUser(UserRegisterDto userRegisterDto);
		Task<UserDto> GetUser(string Token);
		Task<UserDto> UploadImage(ImageUpdateDto img);
	}
}
