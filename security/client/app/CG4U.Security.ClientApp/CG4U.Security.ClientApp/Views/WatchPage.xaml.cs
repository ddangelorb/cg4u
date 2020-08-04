using System;
using System.Collections.Generic;
using System.Net;
using CG4U.Security.ClientApp.Models;
using LibVLCSharp.Shared;
using Xamarin.Forms;
using System.Threading.Tasks;
using LibVLCSharp.Forms.Shared;

namespace CG4U.Security.ClientApp.Views
{
    public partial class WatchPage : ContentPage
    {
        private string _videoStreamingUri;

        public WatchPage()
        {
            InitializeComponent();

            //
            var videoCameras = GetMockCameras();

            NavigationPage.SetHasNavigationBar(this, true);
            Playlist.ItemsSource = videoCameras;
            _videoStreamingUri = videoCameras?.Count > 0 ? videoCameras[0].IngestStreamingUri : string.Empty;
        }

        private List<VideoCamera> GetMockCameras()
        {
            return new List<VideoCamera>()
            {
                new VideoCamera() { Id = 1, Name="Camera 564 externa", Description="Camera 1 da Rua Hugo Garotinho", ImageFile=GetImageMockAsync(1), IngestStreamingUri=@"rtsp://admin:GNC2carotini@prev-hc-gnc-bairro.ddns.net:564"},
                new VideoCamera() { Id = 2, Name="Camera 565 externa", Description="Camera 2 da Rua Hugo Garotinho", ImageFile=GetImageMockAsync(2), IngestStreamingUri=@"rtsp://admin:GNC2carotini@prev-hc-gnc-bairro.ddns.net:565"},
                new VideoCamera() { Id = 3, Name="Camera 564 externa passado", Description="Camera 1 da Rua Hugo Garotinho", ImageFile=GetImageMockAsync(1), IngestStreamingUri=string.Empty},
                new VideoCamera() { Id = 4, Name="Camera 564 interna", Description="Camera 1 da Rua Hugo Garotinho", ImageFile=GetImageMockAsync(1), IngestStreamingUri=@"rtsp://admin:GNC2carotini@192.168.0.71:564"},
                new VideoCamera() { Id = 5, Name="Camera 565 interna", Description="Camera 2 do Oceano", ImageFile=GetImageMockAsync(2), IngestStreamingUri=@"rtsp://admin:GNC2carotini@192.168.0.72:565"},
            };
        }

        private byte[] GetImageMockAsync(int id)
        {
            var filePath = string.Empty;
            if (id == 1) filePath = "http://img.youtube.com/vi/-BrDlrytgm8/default.jpg";
            else if (id == 2) filePath = "http://img.youtube.com/vi/EcOgjrRWx_Q/default.jpg";
            else filePath = "https://img.youtube.com/vi/Xc0d510zTA4/default.jpg";

            var webClient = new WebClient();
            return webClient.DownloadData(filePath);
        }

        private void OnButtonSeekBackwardTapped(object sender, EventArgs e)
        {
            /*
            var awsConfig = new AmazonKinesisVideoArchivedMediaConfig();
            var aws = new AmazonKinesisVideoArchivedMediaClient(awsConfig);
            var ss = await aws.GetHLSStreamingSessionURLAsync(null);
            string videoHLSUri = ss.HLSStreamingSessionURL;
            */
            if (videoLibVlc.MediaPlayer.IsSeekable)
            {
                var s = "OK!";
            }

            try
            {
                videoLibVlc2.MediaPlayer.Play(new Media(videoLibVlc2.LibVLC, @"rtsp://admin:GNC2carotini@prev-hc-gnc-bairro.ddns.net:565", Media.FromType.FromLocation));
                //videoLibVlc.MediaPlayer.Pause();
                //var libVV = videoLibVlc.LibVLC;

                //Content = new StackLayout();
                //var sl = new StackLayout();
                //sl.Children.Add

                /*
                var vv1 = new VideoView()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };
                vv1.MediaPlayer.Play(new Media(libVV, @"rtsp://admin:GNC2carotini@prev-hc-gnc-bairro.ddns.net:564", Media.FromType.FromLocation));

                var vv2 = new VideoView()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };
                vv2.MediaPlayer.Play(new Media(libVV, @"rtsp://admin:GNC2carotini@prev-hc-gnc-bairro.ddns.net:565", Media.FromType.FromLocation));

                layoutVideos.Children.Add(vv1);
                layoutVideos.Children.Add(vv2);
                */
            }
            catch (Exception ex)
            {
                var s = ex.ToString();
            }
        }

        private void OnButtonPlayTapped(object sender, EventArgs e)
        {
            PlayButton.IsVisible = false;
            PauseButton.IsVisible = true;
            videoLibVlc.MediaPlayer.Play(new Media(videoLibVlc.LibVLC, _videoStreamingUri, Media.FromType.FromLocation));
        }

        private void OnButtonPauseTapped(object sender, EventArgs e)
        {
            if (videoLibVlc.MediaPlayer.IsPlaying)
            {
                videoLibVlc.MediaPlayer.Pause();
                PlayButton.IsVisible = true;
                PauseButton.IsVisible = false;
            }
        }

        private void OnButtonSeekForwardTapped(object sender, EventArgs e)
        {
            if (videoLibVlc.MediaPlayer.IsSeekable)
            {
                var s = "OK!";
            }
        }

        private void Playlist_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (videoLibVlc.MediaPlayer.IsPlaying)
            {
                videoLibVlc.MediaPlayer.Stop();
                PlayButton.IsVisible = true;
                PauseButton.IsVisible = false;
            }
            var item = e.SelectedItem as VideoCamera;
            if (item.Id == 3)
                _videoStreamingUri = "https://b-647daf39.kinesisvideo.us-west-2.amazonaws.com/hls/v1/getHLSMasterPlaylist.m3u8?SessionToken=CiABMB9Dxh8vOl_W_eqhTqikmRIKNhCeiiKIzAXkcQDbRRIQgKcBSH0cdDAD-cTPtlRLlhoZ9MALqWkPFZQKb-xt5fp4JhHn8TUlPqFK3SIglHnZVa7Iq69PfXJ5EnI_H2vZ3M5i_aiMxDMlv3-DbUM~";
            else
                _videoStreamingUri = item.IngestStreamingUri;
        }
    }
}
