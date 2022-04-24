using MusicApp.ViewModel;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        //private readonly PlayerViewModel _playerViewModel;
        public PlayerPage()
        {
            InitializeComponent();
            //_playerViewModel = Startup.Resolve<PlayerViewModel>();
            //BindingContext = _playerViewModel;
        }

        public void PlayerPage_AddToFavorite(object sender, FavoritesEventArgs e)
        {
            DisplayAlert("Alert","Added to favorites","Ok");
        }
        public void PlayerPage_OnFailedPlay(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "Failed to play stream!", "Ok");
        }
        public void PlayerPage_OnDownloadClicked(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "Download started to Music folder!", "Ok");
        }
        public async Task PermissionCheckAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await DisplayAlert("Need storage", "Request storage permission", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                //Best practice to always check that the key exists
                if (results == PermissionStatus.Granted)
                    status = PermissionStatus.Granted;
            }
        }
    }
}