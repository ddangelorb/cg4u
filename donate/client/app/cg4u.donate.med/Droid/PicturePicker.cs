using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using CG4U.Donate.ClientApp.Med.Droid;
using CG4U.Donate.ClientApp.Med.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(PicturePicker))]
namespace CG4U.Donate.ClientApp.Med.Droid
{
    public class PicturePicker : IPicturePicker
    {
        public Task<Stream> GetImageStreamAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            return null;
            /*
            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
            */
        }
    }
}
