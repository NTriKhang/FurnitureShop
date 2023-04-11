using Microsoft.AspNetCore.Components;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;

namespace ShopOnline.Solution.Pages.Register
{
	public class RegisterBase : ComponentBase
	{
		[Inject]
		protected IUserServices userServices { set; get; }
		[Inject]
		protected NavigationManager navigationManager { set; get; }
		protected UserRegisterDto userRegisterDto { get; set; } = new UserRegisterDto();

		protected void Register_OnClick()
		{
			try
			{
				if(userRegisterDto != null)
				{
					if (userRegisterDto.Password == userRegisterDto.ConfirmPassword)
					{
						var userDto = userServices.RegisterUser(userRegisterDto);
						navigationManager.NavigateTo("/");
						
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
    }
}
