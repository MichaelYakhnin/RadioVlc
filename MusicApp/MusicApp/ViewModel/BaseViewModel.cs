using MusicApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MusicApp.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// AddToFavorite
        /// </summary>
        public event EventHandler<FavoritesEventArgs> AddToFavorite;

        public event EventHandler FailedPlay;

        public event EventHandler DownloadClicked;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public void OnAddToFavorite(Radio radio)
        {
            AddToFavorite?.Invoke(this, new FavoritesEventArgs(radio));
        }
        public void OnFailedPlay(object sender, EventArgs e)
        {
            FailedPlay?.Invoke(this, new EventArgs());
        }
        public void OnDownloadClicked()
        {
            DownloadClicked?.Invoke(this, new EventArgs());
        }
    }
    public class FavoritesEventArgs : EventArgs
    {
        public Radio SelectedStation { get; set; }
        public FavoritesEventArgs(Radio radio)
        {
            SelectedStation = radio;
        }
    }
}
