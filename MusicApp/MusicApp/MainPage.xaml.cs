using MusicApp.Interfaces;
using MusicApp.ViewModel;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using static MusicApp.Model.Lib;

namespace MusicApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {       
        public MainPage()
        {
            InitializeComponent();
            BindingContext = Startup.Resolve<MainViewModel>();         
        }

        private void ToolbarItemIsrael_Clicked(object sender, EventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            viewModel.GetStationByType("isr");
            viewModel.StationType = StationType.Israel;
            viewModel.GetFavorites(StationType.Israel);
        }

        private void ToolbarItemMoscow_Clicked(object sender, EventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            viewModel.GetStationByType("msk");
            viewModel.StationType = StationType.Russian;
            viewModel.GetFavorites(StationType.Russian);
        }
        private void ToolbarItemKiev_Clicked(object sender, EventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            viewModel.GetStationByType("ukr");
            viewModel.StationType = StationType.Ukraine;
            viewModel.GetFavorites(StationType.Ukraine);
        }
        private void ToolbarItemStationsUpdate_Clicked(object sender, EventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;
            viewModel.GetRadioListUpdate();           
        }
        protected override bool OnBackButtonPressed()
        {
            if (Application.Current.MainPage.Navigation.NavigationStack.Count == 1)//navigation is MainPage.Navigation
                DependencyService.Get<IAndroidMethods>().CloseApp();
            return base.OnBackButtonPressed();
        }

    }
    
}
