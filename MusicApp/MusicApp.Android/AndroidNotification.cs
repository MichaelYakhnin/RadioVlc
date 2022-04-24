using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.App;
using MusicApp.Services;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(MusicApp.Droid.AndroidNotification))]
namespace MusicApp.Droid
{
    public class AndroidNotification : ICustomNotification
    {
        public static String NOTIFICATION_CHANNEL_ID = "10023";
        private Context mContext;
        private NotificationCompat.Builder mBuilder;
        public AndroidNotification()
        {
            mContext = Android.App.Application.Context; 
        }

        [System.Obsolete]
        public void send(string title, string message)
        {
            try
            {
                // Set up an intent so that tapping the notifications returns to this app:
                var intent = new Intent(mContext, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra(title, message);
                var pendingIntent = PendingIntent.GetActivity(mContext, 1, intent, PendingIntentFlags.UpdateCurrent);
                mBuilder = new NotificationCompat.Builder(mContext);
                mBuilder
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetChannelId(NOTIFICATION_CHANNEL_ID)
                    .SetLargeIcon(BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, Resource.Drawable.favorite))
                    .SetSmallIcon(Resource.Drawable.exo_icon_play)
                    .SetPriority((int)NotificationPriority.High)
                    .SetVisibility((int)NotificationVisibility.Public)
                    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);


                NotificationManager notificationManager = mContext.GetSystemService(Context.NotificationService) as NotificationManager;

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    NotificationImportance importance = Android.App.NotificationImportance.High;

                    NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, title, importance);
                    notificationChannel.EnableLights(true);
                    notificationChannel.EnableVibration(true);
                    //notificationChannel.SetSound(sound, alarmAttributes);
                    notificationChannel.SetShowBadge(true);
                    notificationChannel.Importance = NotificationImportance.High;
                    //notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                    if (notificationManager != null)
                    {
                        mBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                        notificationManager.CreateNotificationChannel(notificationChannel);
                    }
                }

                notificationManager.Notify(0, mBuilder.Build());
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}