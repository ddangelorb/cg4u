using System;
using LibVLCSharp.Shared;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Views
{
    public partial class VideoPage : ContentPage
    {
        public VideoPage()
        {
            InitializeComponent();
        }

        //http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4

        /*protected override void OnAppearing()
        {
            base.OnAppearing();

            videoView.MediaPlayer.Play(new Media(videoView.LibVLC,
                "https://b-647daf39.kinesisvideo.us-west-2.amazonaws.com/hls/v1/getHLSMasterPlaylist.m3u8?SessionToken=CiA6MSLFbSuzU0azrPrfpwcIdHCIlAubLE9x7v9gdOBRQRIQ9DbB5ZIKduz2m42P_bYgxRoZcWU2nWrOypYFAyx6XvcqZ1FoKIfDSUhiWCIgEGj8rvZiAQvIoD40GS2_-3LWgIKwzDnwWFWzLRhW5l8~"
                , Media.FromType.FromLocation));
        }*/
    }
}
