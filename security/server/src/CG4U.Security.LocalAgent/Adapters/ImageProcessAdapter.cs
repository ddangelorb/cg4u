using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CG4U.Security.LocalAgent.Datas;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CG4U.Security.LocalAgent.Adapters
{
    public class ImageProcessAdapter : RunnableAdapter
    {
        private ImageProcessApiData _imageProcessApiData;
        private FileSystemWatcher _fileSystemWatcherThumbnail;
        private FileSystemWatcher _fileSystemWatcherSceneChange;

        public ImageProcessAdapter(ILogger logger, ImageProcessApiData imageProcessApiData, VideoCameraData videoCameraData, EmailSenderData emailSenderData)
            : base(emailSenderData, logger, videoCameraData)
        {
            _imageProcessApiData = imageProcessApiData;

            _fileSystemWatcherThumbnail = new FileSystemWatcher();
            _fileSystemWatcherThumbnail.Filter = "*.png";
            _fileSystemWatcherThumbnail.EnableRaisingEvents = true;

            _fileSystemWatcherSceneChange = new FileSystemWatcher();
            _fileSystemWatcherSceneChange.Filter = "*.png";
            _fileSystemWatcherSceneChange.EnableRaisingEvents = true;
        }

        public void CustomAnalyze()
        {
            try
            {
                if (_videoCameraData.IsImageProcessThumbnailActive)
                {
                    _fileSystemWatcherThumbnail.Path = _videoCameraData.ThumbnailImagePath;
                    _fileSystemWatcherThumbnail.Created += new FileSystemEventHandler(OnThumbnailFileCreatedAsync);
                }

                if (_videoCameraData.IsImageProcessSceneChangeActive)
                {
                    _fileSystemWatcherSceneChange.Path = _videoCameraData.SceneChangeImagePath;
                    _fileSystemWatcherSceneChange.Created += new FileSystemEventHandler(OnSceneChangeFileCreatedAsync);
                }

                while (_isRunning) {};
            }
            catch (Exception ex)
            {
                ReportError(ex, "ImageProcessAdapter.CustomAnalyze Error", string.Concat($"ImageProcessAdapter.CustomAnalyze raised an exception with DeviceName: '{_videoCameraData.DeviceName}, Outputfile: '{_videoCameraData.OutputFilePath}'", ex.Message, "::", ex.StackTrace));
            }
        }

        private async void OnThumbnailFileCreatedAsync(object source, FileSystemEventArgs e)
        {
            var isApiReturnSucess = await IsApiReturnSucess(e, "ImageProcessAdapter.OnThumbnailFileCreatedAsync");
            if (isApiReturnSucess)
                File.Move(e.FullPath, _videoCameraData.ThumbnailImageProcessedFolder);
        }

        private async void OnSceneChangeFileCreatedAsync(object source, FileSystemEventArgs e)
        {
            var isApiReturnSucess = await IsApiReturnSucess(e, "ImageProcessAdapter.OnSceneChangeFileCreatedAsync");
            if (isApiReturnSucess)
                File.Move(e.FullPath, _videoCameraData.SceneChangeImageProcessedFolder);
        }

        private async Task<bool> IsApiReturnSucess(FileSystemEventArgs e, string originMethod)
        {
            using (var apiClient = new HttpClient())
            {
                apiClient.BaseAddress = new Uri(_imageProcessApiData.BaseAddress);
                using (var postContent = new StringContent(JsonConvert.SerializeObject(GetPostContentData(e)), Encoding.UTF8, "application/json"))
                using (var response = await apiClient.PostAsync(_imageProcessApiData.AddByVideoCameraAsync, postContent))                    
                {
                    if (!response.IsSuccessStatusCode)
                        ReportError(null, string.Concat(originMethod, " Api.AddByVideoCamera Error"), $"Response error reported:: File:'{e.FullPath}', {response.StatusCode}, '{response.ToString()}'");

                    return response.IsSuccessStatusCode;
                }
            }
        }

        private object GetPostContentData(FileSystemEventArgs e)
        {
            using (var file = File.OpenRead(e.FullPath))
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);

                return new
                {
                    IdVideoCameras = _videoCameraData.Id,
                    ImageFile = ms.ToArray(),
                    ImageName = e.Name,
                    IpUserRequest = string.Empty,
                    VideoPath = _videoCameraData.OutputFilePath,
                    SecondsToStart = Regex.Match(e.Name, @"\d+").Success ? Int32.Parse(Regex.Match(e.Name, @"\d+").Value)/30 : 0,
                    DtProcess = DateTime.Now,
                    Active = 1
                };
            } 
        }
    }
}
