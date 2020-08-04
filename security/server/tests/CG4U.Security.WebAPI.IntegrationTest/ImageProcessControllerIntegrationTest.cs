using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Security.WebAPI.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace CG4U.Security.WebAPI.IntegrationTest
{
    public class ImageProcessControllerIntegrationTest : BaseControllerIntegrationTest
    {
        public ImageProcessControllerIntegrationTest()
        {
        }

        [Fact(DisplayName = "01 : Get an Unauthorized HttpStatus")]
        [Trait("Category", "ImageProcessController.Security.WebAPI.IntegrationTest")]
        public async Task ImageProcessController_AddByVideoCameraAsync_Unauthorized()
        {
            //Arrange
            var imageProcess = new ImageProcessViewModel()
            {
                IdVideoCameras = 1,
                IdReference = Guid.NewGuid(),
                ImageFile = null,
                ImageName = "",
                IpUserRequest = "200.111.222.33",
                VideoPath = "/user/danieldangeloresendebarros/opt",
                SecondsToStart = 15,
                DtProcess = DateTime.Now
            };

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(imageProcess), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiSecurity
                .CreateRequest("ImageProcess/AddByVideoCameraAsync")
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact(DisplayName = "02 : Add ImageProcess By VideoCamera")]
        [Trait("Category", "ImageProcessController.Security.WebAPI.IntegrationTest")]
        public async Task ImageProcessController_AddByVideoCameraAsync_Successfully()
        {
            //Arrange
            var imageProcess = new ImageProcessViewModel()
            {
                IdVideoCameras = 1,
                IdReference = Guid.NewGuid(),
                ImageName = "out000090.png",
                IpUserRequest = "200.111.222.33",
                VideoPath = "/user/danieldangeloresendebarros/opt",
                SecondsToStart = 15,
                DtProcess = DateTime.Now
            };
            using (var sc = new StreamContent(File.OpenRead($"Img/Thumbnail/{imageProcess.ImageName}")))
            using (var ms = new MemoryStream())
            {
                await sc.CopyToAsync(ms);
                imageProcess.ImageFile = ms.ToArray();
            }

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(imageProcess), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiSecurity
                .CreateRequest("ImageProcess/AddByVideoCameraAsync")
                .AddHeader("Authorization", "Bearer " + _loginAdm.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
