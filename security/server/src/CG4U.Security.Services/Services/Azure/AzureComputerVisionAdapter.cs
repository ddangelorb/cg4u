using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CG4U.Security.Services.Data;
using CG4U.Security.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CG4U.Security.Services.Services.Azure
{
    public class AzureComputerVisionAdapter : AzureBaseAdapter, IComputerVisionAdapter
    {
        private AzureApiData _azureApiData;

        public AzureComputerVisionAdapter(IOptions<AzureApiData> azureApiData, ILogger logger)
            : base(logger)
        {
            _azureApiData = azureApiData.Value;
        }

        public async Task<string> DescribeImageAsync(string language, byte[] image, ConnectionApiData connectionApiData)
        {
            var uri = string.Format(_azureApiData.DescribeImageUri, "1", language);
            using (var azureClientApi = GetAzureApiClient(connectionApiData))
            using (var postContent = new ByteArrayContent(image))
            {
                postContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                using (var response = await azureClientApi.PostAsync(uri, postContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(string.Concat("AzureComputerVisionAdapter.DescribeImageAsync::Cannot describe image", response.ToString()));
                        return null;
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<string> OCRAsync(string language, byte[] image, ConnectionApiData connectionApiData)
        {
            var uri = string.Format(_azureApiData.OCRUri, language);
            using (var azureClientApi = GetAzureApiClient(connectionApiData))
            using (var postContent = new ByteArrayContent(image))
            {
                postContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                using (var response = await azureClientApi.PostAsync(uri, postContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(string.Concat("AzureComputerVisionAdapter.OCRAsync::Cannot OCR image", response.ToString()));
                        return null;
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<string> RecognizeTextAsync(string mode, byte[] image, ConnectionApiData connectionApiData)
        {
            var uri = string.Format(_azureApiData.RecognizeTextUri, mode);
            using (var azureClientApi = GetAzureApiClient(connectionApiData))
            using (var postContent = new ByteArrayContent(image))
            {
                postContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                using (var response = await azureClientApi.PostAsync(uri, postContent))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(string.Concat("AzureComputerVisionAdapter.RecognizeTextAsync::Cannot Recognize Text image", response.ToString()));
                        return null;
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
