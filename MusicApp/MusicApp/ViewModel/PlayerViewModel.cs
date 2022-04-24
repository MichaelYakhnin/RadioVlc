using LibVLCSharp.Shared;
using MediaManager;
using MediaManager.Media;
using MusicApp.Model;
using MusicApp.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace MusicApp.ViewModel
{
    public class PlayerViewModel : BaseViewModel
    {
        private MediaPlayer mediaPlayer;
        private PlaybackService _playbackService;
        public PlayerViewModel(Radio selectedMusic, ObservableCollection<Radio> musicList, PlaybackService playbackService)
        {
            this.selectedMusic = selectedMusic;
            this.musicList = musicList;
            _playbackService = playbackService; 
            PlayMusic(selectedMusic);
            isPlaying = true;          
        }

        #region Properties
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

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan position;
        public TimeSpan Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }
        public double TotalSeconds;
        double maximum = 100f;
        public double Maximum
        {
            get { return maximum; }
            set
            {
                if (value > 0)
                {
                    maximum = value;
                    OnPropertyChanged(); 
                }
            }
        }


        private bool isPlaying;
        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PlayIcon));
            }
        }
        private bool isRecording;
        public bool IsRecording
        {
            get { return isRecording; }
            set
            {
                isRecording = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RecordIcon));
            }
        }

        public string PlayIcon { get => isPlaying ? "pause.png" : "play.png"; }
        public string RecordIcon { get => isRecording ? "stop.png" : "record.png"; }

        #endregion

        public ICommand PlayCommand => new Command(Play);
        public ICommand ChangeCommand => new Command(ChangeMusic);
        public ICommand BackCommand => new Command(() => Application.Current.MainPage.Navigation.PopAsync());
        public ICommand RecordCommand => new Command(() => Download());

        public ICommand AddToFavoriteCommand => new Command(() => OnAddToFavorite(selectedMusic));

        private void Play()
        {
            if(isPlaying)
            {
                // await CrossMediaManager.Current.Pause();
                _playbackService.Play(false);
                IsPlaying = false; 
            }
            else
            {
                //await CrossMediaManager.Current.Play();
                _playbackService.Play(true);
                IsPlaying = true; 
            }
        }
        
        private void ChangeMusic(object obj)
        {
            if ((string)obj == "P")
                PreviousMusic();
            else if ((string)obj == "N")
                NextMusic();
        }

        private void PlayMusic(Radio music)
        {
            _playbackService.Init(music?.Src);
            //var mediaInfo = CrossMediaManager.Current;
            //await mediaInfo.Stop();
            //await mediaInfo.Play(music?.Src);
            
            Play();
            //IsPlaying = true;

            //CrossMediaManager.Current.MediaItemFailed += OnFailedPlay;
            _playbackService._mp.EncounteredError += OnFailedPlay;
            //CrossMediaManager.Current.MediaItemChanged += (object sender, MediaItemEventArgs e) => {
            //    e.MediaItem.DisplayTitle = music.Title;
            //    // Access any other metadata property through the file
            //};

            //CrossMediaManager.Current.Notification.ShowNavigationControls = false;

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () => 
            {
                Duration = new TimeSpan(0,59,59);
                Maximum = duration.TotalSeconds;
                Position = new TimeSpan(0, 0, 0);//mediaInfo.Position;
                TotalSeconds = 600;//mediaInfo.Position.TotalSeconds;
                return true;
            });
        }

        

        private void NextMusic()
        {
            var currentIndex = musicList.IndexOf(selectedMusic);

            if (currentIndex < musicList.Count - 1)
            {
                SelectedMusic = musicList[currentIndex + 1];
                PlayMusic(selectedMusic);
            }
        }

        private void PreviousMusic()
        {
            var currentIndex = musicList.IndexOf(selectedMusic);

            if (currentIndex > 0)
            {
                SelectedMusic = musicList[currentIndex - 1];
                PlayMusic(selectedMusic);
            }
        }
        private void Download()
        {
            if(isRecording)
            {
                mediaPlayer.Stop();
                mediaPlayer = null;
                IsRecording = false;
            }
            else
            {
                //Core.Initialize();
                using (var libvlc = new LibVLC())
                {
                    mediaPlayer = new MediaPlayer(libvlc);
                    var media = new Media(libvlc, selectedMusic.Src, FromType.FromLocation);
                    var currentDirectory = "/storage/emulated/0/music/";
                    var destination = Path.Combine(currentDirectory, $"record-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.mp3");
                    // Define stream output options. 
                    // In this case stream to a file with the given path and play locally the stream while streaming it
                    media.AddOption(":enable:sout");
                    media.AddOption(":sout=#transcode{acodec=mp3}:std{access=file,dst=" + destination + "}");
                    // Start recording
                    mediaPlayer.Play(media);
                    IsRecording = true;
                    OnDownloadClicked();
                }
            }
            
        }
       
    }
}