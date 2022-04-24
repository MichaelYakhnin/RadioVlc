using Android.App;
using MediaManager;
using MediaManager.Player;
using MusicApp.Droid;
using MusicApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace MusicApp.Droid
{
    public class AndroidMethods : IAndroidMethods
    {
        [System.Obsolete]
        public void CloseApp()
        {
            //CrossMediaManager.Current.MediaPlayer.Stop();

            //if (CrossMediaManager.Current.Notification != null)
            //{
            //    CrossMediaManager.Current.Notification.Enabled = false;
            //}
            (Xamarin.Forms.Forms.Context as Activity).Finish();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());           
        }
    }
}