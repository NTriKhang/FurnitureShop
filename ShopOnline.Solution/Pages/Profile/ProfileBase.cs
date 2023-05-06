using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ShopOnline.Models.dtos;
using ShopOnline.Solution.Services.Contract;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace ShopOnline.Solution.Pages.Profile
{
    public class ProfileBase : ComponentBase
    {
        [Inject]
        private IUserServices userServices { set; get; }
        [Inject]
        private ILocalStorageService localStorageService { set; get; }
        [Inject]
        private NavigationManager navigationManager { set; get; }
        protected UserDto user { get; set; } = new UserDto();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var Token = await localStorageService.GetItemAsync<string>(utility.TokenJwt);
                user = await userServices.GetUser(Token);
                if (user.ImageUrl == string.Empty)
                {
                    user.ImageUrl = "\\Images\\hinh-meme-meo-cuoi-deu.png";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task UpLoad_Img(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if(file != null)
            {
                var resizeFile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                var buff = new byte[resizeFile.Size];
                using(var stream = resizeFile.OpenReadStream())
                {
                    await stream.ReadAsync(buff);
                }
                var fileUpdate = new ImageUpdateDto()
                {
                    fileName = file.Name,
                    base64data = Convert.ToBase64String(buff),
                    token = user.Token,
                };
                var result = await userServices.UploadImage(fileUpdate);
                user.ImageUrl = result.ImageUrl;
            }

		}


    }

}
