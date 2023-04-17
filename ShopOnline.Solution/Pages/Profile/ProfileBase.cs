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
        private HttpClient httpClient { set; get; }
        protected UserDto user { get; set; } = new UserDto();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var UserName = await localStorageService.GetItemAsStringAsync(utility.UserName);
                user = await userServices.GetUserByName(UserName);
                if (user.ImageUrl == string.Empty)
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected List<string> fileName = new List<string>();
        int maxAllowedFiles = 1;
        long maxFileSize = int.MaxValue;
        protected async Task UpLoad_Img(InputFileChangeEventArgs e)
        {
            using var content = new MultipartFormDataContent();
            foreach(var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                var fileContent = new StreamContent(file.OpenReadStream(maxFileSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                fileName.Add(file.Name);
                content.Add(
                    content: fileContent,
					name: "\"files\"",
					fileName: file.Name);
            }
            var tmp = await userServices.UploadImage(content);

		}


    }

}
