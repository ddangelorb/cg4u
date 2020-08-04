using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CG4U.Security.LocalAgent.Datas;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.LocalAgent.Adapters
{
    public class FFmpegAdapter : RunnableAdapter
    {
        private FFmpegData _ffmpegData;
        private string _deviceName;
        private string _outputFolderPath;
        private string _outputFilePath;

        public FFmpegAdapter(ILogger logger, FFmpegData ffmpegData, VideoCameraData videoCameraData, EmailSenderData emailSenderData)
            : base(emailSenderData, logger, videoCameraData)
        {
            _ffmpegData = ffmpegData;

            _deviceName = _videoCameraData.DeviceName;
            _outputFolderPath = string.Format(_videoCameraData.OutputFolderPath, _deviceName);
            _outputFilePath = string.Format(_videoCameraData.OutputFilePath, _outputFolderPath, _deviceName);
        }

        public void CaptureCamera()
        {
            var commandPattern = _ffmpegData.CaptureCameraCommand;
            var messageLog = $"FFmpegAdapter.CaptureCamera with DeviceName: '{_deviceName}, Outputfile: '{_outputFilePath}' and Command: '{commandPattern}'";

            RunProcess(messageLog, commandPattern, _deviceName, _outputFilePath);
        }

        public void StreamingCamera()
        {
            var ingestStreamingUri = _videoCameraData.IngestStreamingUri;
            var commandPattern = _ffmpegData.StreamingCommand;
            var messageLog = $"FFmpegAdapter.StreamingCamera with DeviceName: '{_deviceName}, Outputfile: '{_outputFilePath}', IngestStreamingUri: '{ingestStreamingUri}' and Command: '{commandPattern}'";

            while (!File.Exists(_outputFilePath))
            {
                Thread.Sleep(_ffmpegData.WaitingTimeOutputFileInMilisecs);
                _logger.LogWarning(string.Concat("Waiting outputFile creation::", messageLog));
            }

            RunProcess(messageLog, commandPattern, _outputFilePath, ingestStreamingUri);
        }

        public void ThumbnailImageCamera()
        {
            var thumbnailImageIntervalInSeconds = _videoCameraData.ThumbnailImageIntervalInSeconds;
            var thumbnailImagePath = string.Format(_videoCameraData.ThumbnailImagePath, _outputFolderPath);
            var commandPattern = _ffmpegData.ThumbnailImageCommand;
            var messageLog = $"FFmpegAdapter.ThumbnailImageCamera with DeviceName: '{_deviceName}, Outputfile: '{_outputFilePath}', ThumbnailImageIntervalInSeconds: '{thumbnailImageIntervalInSeconds}',  ThumbnailImagePath: '{thumbnailImagePath}' and Command: '{commandPattern}'";

            while (!File.Exists(_outputFilePath))
            {
                Thread.Sleep(_ffmpegData.WaitingTimeOutputFileInMilisecs);
                _logger.LogWarning(string.Concat("Waiting outputFile creation::", messageLog));
            }

            RunProcess(messageLog, commandPattern, _outputFilePath, thumbnailImageIntervalInSeconds, thumbnailImagePath);
        }

        public void SceneChangeCamera()
        {
            var sceneChangePercentThreshold = _videoCameraData.SceneChangePercentThreshold;
            var sceneChangeImagePath = string.Format(_videoCameraData.SceneChangeImagePath, _outputFolderPath);
            var commandPattern = _ffmpegData.SceneChangeCommand;
            var messageLog = $"FFmpegAdapter.SceneChangeCamera with DeviceName: '{_deviceName}, Outputfile: '{_outputFilePath}', SceneChangePercentThreshold: '{sceneChangePercentThreshold}', SceneChangeImagePath: '{sceneChangeImagePath}' and Command: '{commandPattern}'";

            while (!File.Exists(_outputFilePath))
            {
                Thread.Sleep(_ffmpegData.WaitingTimeOutputFileInMilisecs);
                _logger.LogWarning(string.Concat("Waiting outputFile creation::", messageLog));
            }

            RunProcess(messageLog, commandPattern, _outputFilePath, sceneChangePercentThreshold, sceneChangeImagePath);
        }

        private void RunProcess(string messageLog, string commandPattern, params object[] commandArgs)
        {
            var process = new Process();
            try
            {
                var command = string.Format(commandPattern, commandArgs);
                if (StartProcess(messageLog, process, command))
                {
                    process.WaitForExit();

                    while (_isRunning) { };

                    process.Close();
                    _logger.LogInformation(string.Concat("Process ran successfully::", messageLog));
                }
            }
            catch (Exception ex)
            {
                ReportError(ex, "ProcessAdapter.Run Error", string.Concat("Raised an exception::", messageLog, "::", ex.Message, "::", ex.StackTrace));
            }
            finally
            {
                process.Kill();
                _logger.LogInformation(string.Concat("Process killed::", messageLog));
            }
        }

        private bool StartProcess(string messageLog, Process process, string command)
        {
            _logger.LogInformation(string.Concat("Process about to start::", messageLog));

            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = "";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            if (process.Start()) return true;

            _logger.LogError(string.Concat("Process could not be started::", messageLog));
            return false;
        }
    }
}
