namespace CG4U.Security.LocalAgent.Datas
{
    public class VideoCameraData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string DeviceName { get; set; }
        public string OutputFolderPath { get; set; }
        public string OutputFilePath { get; set; }
        public bool IsStreamingActive { get; set; }
        public bool IsImageProcessThumbnailActive { get; set; }
        public bool IsImageProcessSceneChangeActive { get; set; }
        public bool IsImageProcessCustomAnalyzeActive { get; set; }
        public string ThumbnailImagePath { get; set; }
        public string ThumbnailImageProcessedFolder { get; set; }
        public int ThumbnailImageIntervalInSeconds { get; set; }
        public string IngestStreamingUri { get; set; }
        public string SceneChangePercentThreshold { get; set; }
        public string SceneChangeImagePath { get; set; }
        public string SceneChangeImageProcessedFolder { get; set; }
    }
}
