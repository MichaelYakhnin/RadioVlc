using MusicApp.Logic;
using MusicApp.Model;
using MusicApp.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static MusicApp.Model.Lib;

namespace MusicApp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        PlayerPage playerPage;
        private static object _lock = new object();
        public StationType StationType { get; set; }
        private readonly IRadioService _radioService;
        ICustomNotification notification;
        private PlaybackService _playbackService;
        public MainViewModel() { }
        public MainViewModel(IRadioService radioService, PlaybackService playbackService)
        {
            _radioService = radioService;
            GetStationByType("isr");
            notification = DependencyService.Get<ICustomNotification>();
            _playbackService = playbackService;
        }

        ObservableCollection<Radio> musicList;
        public ObservableCollection<Radio> MusicList
        {
            get { return musicList; }
            set
            {
                musicList = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<Radio> favoritesList = new ObservableCollection<Radio>();
        public ObservableCollection<Radio> FavoritesList
        {
            get { return favoritesList; }
            set
            {
                favoritesList = value;
                OnPropertyChanged();
            }
        }

        private Radio recentMusic;
        public Radio RecentMusic
        {
            get { return recentMusic; }
            set
            {
                recentMusic = value;
                OnPropertyChanged();
            }
        }

        private Radio selectedMusic;
        public Radio SelectedMusic
        {
            get { return selectedMusic; }
            set
            {
                selectedMusic = value;
                OnPropertyChanged();
            }
        }
        private Radio selectedFavorite;
        public Radio SelectedFavorite
        {
            get { return selectedFavorite; }
            set
            {
                selectedFavorite = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectionCommand => new Command(PlayMusic);
        public ICommand BackCommand => new Command(() => Application.Current.MainPage.Navigation.PushAsync(playerPage, true));

        private void PlayMusic(object obj)
        {
            if((string)obj == "Favorite" && selectedFavorite != null)
            {
                RecentMusic = selectedFavorite;
                
                var viewModel = new PlayerViewModel(selectedFavorite, favoritesList, _playbackService);
                playerPage = new PlayerPage { BindingContext = viewModel };
                playerPage.PermissionCheckAsync().Wait();
                var navigation = Application.Current.MainPage as NavigationPage;
                navigation.PopAsync();
                navigation.PushAsync(playerPage, true);

                viewModel.FailedPlay += playerPage.PlayerPage_OnFailedPlay;
                viewModel.DownloadClicked += playerPage.PlayerPage_OnDownloadClicked;
                notification.send(RecentMusic.Title, RecentMusic.Name);
            }
            if ((string)obj == "Regular" && selectedMusic != null)
            {
                RecentMusic = selectedMusic;
                
                var viewModel = new PlayerViewModel(selectedMusic, musicList, _playbackService) ;
                playerPage = new PlayerPage { BindingContext = viewModel };
                playerPage.PermissionCheckAsync().Wait();
                viewModel.AddToFavorite += OnAddToFavoriteAsync;
               
                var navigation = Application.Current.MainPage as NavigationPage;
                navigation.PopAsync();
                navigation.PushAsync(playerPage, true);
                viewModel.AddToFavorite += playerPage.PlayerPage_AddToFavorite;
                viewModel.FailedPlay += playerPage.PlayerPage_OnFailedPlay;
                viewModel.DownloadClicked += playerPage.PlayerPage_OnDownloadClicked;
                notification.send(RecentMusic.Title, RecentMusic.Name);
            }
        }

        private void OnAddToFavoriteAsync(object sender, FavoritesEventArgs e)
        {
            switch (StationType)
            {
                case StationType.Israel:
                    if (!FavoritesList.Any(x => x == e.SelectedStation))
                    {
                        lock (_lock)
                        {
                            FavoritesList.Add(e.SelectedStation);
                            var list = string.Join(",", FavoritesList.Select(x => x.Name));
                            Settings.IsrFavoriteSettings = list;
                        }
                                                                   
                    }
                    break;
                case StationType.Russian:
                    if (!FavoritesList.Any(x => x == e.SelectedStation))
                    {
                        lock (_lock)
                        {
                            FavoritesList.Add(e.SelectedStation);
                            var listRu = string.Join(",", FavoritesList.Select(x => x.Name));
                            Settings.RuFavoriteSettings = listRu;
                        }                       
                    }
                    break;
                case StationType.Ukraine:
                    if (!FavoritesList.Any(x => x == e.SelectedStation))
                    {
                        lock (_lock)
                        {
                            FavoritesList.Add(e.SelectedStation);
                            var listRu = string.Join(",", FavoritesList.Select(x => x.Name));
                            Settings.UkrFavoriteSettings = listRu;
                        }                       
                    }
                    break;
                default:
                    break;
            }
        }

        public void GetFavorites(StationType stationType)
        {
            switch (stationType)
            {
                case StationType.Israel:
                    var list = MusicList.Where(x => Settings.IsrFavoriteSettings.Split(',').Any(y => y == x.Name)).ToList();
                    FavoritesList = new ObservableCollection<Radio>(list);
                    break;
                case StationType.Russian:
                    var listRu = MusicList.Where(x => Settings.RuFavoriteSettings.Split(',').Any(y => y == x.Name));
                    FavoritesList = new ObservableCollection<Radio>(listRu);
                    break;
                case StationType.Ukraine:
                    var listUkr = MusicList.Where(x => Settings.UkrFavoriteSettings.Split(',').Any(y => y == x.Name));
                    FavoritesList = new ObservableCollection<Radio>(listUkr);
                    break;
                default:
                    break;
            }
           
        }

        public async void GetStationByType(string country)
        {
            MusicList = await _radioService.GetStationsListLocal($"{country}.json").ConfigureAwait(false);
           
            GetFavorites(StationType.Israel);
            recentMusic = musicList.FirstOrDefault();
            StationType = StationType.Israel;
        }
      
        
        public async void GetRadioListUpdate()
        {
           var ukrList = await _radioService.GetJsonFromGithub("ukr.json");
           var isrList = await _radioService.GetJsonFromGithub("isr.json");
           var rusList = await _radioService.GetJsonFromGithub("rus.json");
            switch (StationType)
            {
                case StationType.Israel:
                    MusicList = isrList;
   
                    break;
                case StationType.Russian:
                    MusicList = rusList;
  
                    break;
                case StationType.Ukraine:
                    MusicList = ukrList;
    
                    break;
                default:
                    break;
            }
        }
    }
}