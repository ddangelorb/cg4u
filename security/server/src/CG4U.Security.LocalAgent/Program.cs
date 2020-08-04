using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CG4U.Security.LocalAgent.Adapters;
using CG4U.Security.LocalAgent.Datas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CG4U.Security.LocalAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            var serviceProvider = BuildDi();
            var logger = serviceProvider.GetService<ILogger<Program>>();

            var runnableAdapters = new List<RunnableAdapter>();
            var emailSenderData = configuration.GetSection("EmailSenderData").Get<EmailSenderData>();
            var ffmpegData = configuration.GetSection("FFmpegData").Get<FFmpegData>();
            var imageProcessApiData = configuration.GetSection("ImageProcessApiData").Get<ImageProcessApiData>();
            foreach (var vcdc in configuration.GetSection("VideoCameraData").GetChildren())
            {
                var videoCamera = vcdc.Get<VideoCameraData>();
                if (videoCamera.Active)
                {
                    logger.LogInformation($"Starting VideoCamera: '{videoCamera.Name}' processes...");

                    var ffmpegAdapter = new FFmpegAdapter(logger, ffmpegData, videoCamera, emailSenderData);
                    runnableAdapters.Add(ffmpegAdapter);
                    var imageProcessAdapter = new ImageProcessAdapter(logger, imageProcessApiData, videoCamera, emailSenderData);
                    runnableAdapters.Add(imageProcessAdapter);

                    //video camera capturing
                    logger.LogInformation($"Starting VideoCamera: '{videoCamera.Name}' capturing process...");
                    var threadCaptureCamera = new Thread(new ThreadStart(ffmpegAdapter.CaptureCamera));
                    threadCaptureCamera.Priority = ThreadPriority.Highest;
                    threadCaptureCamera.Start();
                    threadCaptureCamera.Join();

                    //video camera streaming
                    if (videoCamera.IsStreamingActive)
                    {
                        logger.LogInformation($"Starting VideoCamera: '{videoCamera.Name}' streming process...");
                        var threadStreming = new Thread(new ThreadStart(ffmpegAdapter.StreamingCamera));
                        threadStreming.Priority = ThreadPriority.AboveNormal;
                        threadStreming.Start();
                        threadStreming.Join();
                    }

                    //video camera thumbnail collecting
                    if (videoCamera.IsImageProcessThumbnailActive)
                    {
                        logger.LogInformation($"Starting VideoCamera: '{videoCamera.Name}' thumbnail image process...");
                        var threadThumbnailImage = new Thread(new ThreadStart(ffmpegAdapter.ThumbnailImageCamera));
                        threadThumbnailImage.Priority = ThreadPriority.Normal;
                        threadThumbnailImage.Start();
                        threadThumbnailImage.Join();
                    }

                    //video camera scene changing
                    if (videoCamera.IsImageProcessSceneChangeActive)
                    {
                        logger.LogInformation($"Starting VideoCamera: '{videoCamera.Name}' scene change capture process...");
                        var threadSceneChange = new Thread(new ThreadStart(ffmpegAdapter.SceneChangeCamera));
                        threadSceneChange.Priority = ThreadPriority.Normal;
                        threadSceneChange.Start();
                        threadSceneChange.Join();
                    }

                    //video camera custom analyzing (according to db config: face and/or computer vision)
                    if (videoCamera.IsImageProcessCustomAnalyzeActive)
                    {
                        logger.LogInformation($"Starting VideoCamera: '{videoCamera.Name}' image analyze process...");
                        var threadImageAnalyze = new Thread(new ThreadStart(imageProcessAdapter.CustomAnalyze));
                        threadImageAnalyze.Priority = ThreadPriority.BelowNormal;
                        threadImageAnalyze.Start();
                        threadImageAnalyze.Join();
                    }
                }
            }

            Console.WriteLine("Press \'q\' to quit.");
            while (Console.Read() != 'q') { };

            foreach (var adapter in runnableAdapters)
                adapter.StopRun();

            NLog.LogManager.Shutdown(); 
        }

        private static ServiceProvider BuildDi()
        {
            var services = new ServiceCollection();

            services.AddTransient<FFmpegAdapter>();
            services.AddTransient<ImageProcessAdapter>();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //configure NLog
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");

            return serviceProvider;
        }
    }
}
