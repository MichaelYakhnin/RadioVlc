using Microsoft.Extensions.DependencyInjection;
using MusicApp.Services;
using MusicApp.ViewModel;
using System;


namespace MusicApp
{
    public static class Startup
    {
        private static IServiceProvider serviceProvider;
        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            //add services
            services.AddHttpClient<IRadioService, ApiRadioService>(c => 
            {
                c.BaseAddress = new Uri("https://michaelyakhnin.github.io/radio-app/src/assets/mobile/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            //add viewmodels
            services.AddSingleton<PlayerViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<PlaybackService>();

            serviceProvider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => serviceProvider.GetService<T>();
    }
}
