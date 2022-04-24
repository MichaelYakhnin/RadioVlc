using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MusicApp.Logic
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string IsrFavoritesKey = "isr_favorites_key";
        
        private const string RuFavoritesKey = "ru_favorites_key";
        
        private const string UkrFavoritesKey = "ukr_favorites_key";
        
        private static readonly string SettingsDefault = string.Empty;

        #endregion
        public static void SaveStationsSettings(string countryKey, string value)
        {
            AppSettings.AddOrUpdateValue(countryKey, value);
        }
        public static string GetStationsSettings(string countryKey)
        {
            if (AppSettings.Contains(countryKey))
              return AppSettings.GetValueOrDefault(countryKey, SettingsDefault);
            return null;
        }
        

        public static string IsrFavoriteSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsrFavoritesKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsrFavoritesKey, value);
            }
        }
        public static string RuFavoriteSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(RuFavoritesKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RuFavoritesKey, value);
            }
        }
        public static string UkrFavoriteSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(UkrFavoritesKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UkrFavoritesKey, value);
            }
        }

    }
}

