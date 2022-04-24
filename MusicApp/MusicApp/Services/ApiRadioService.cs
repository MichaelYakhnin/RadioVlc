using MusicApp.Logic;
using MusicApp.Model;
using MusicApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MusicApp.Services
{
    public class ApiRadioService : IRadioService
    {
        private readonly HttpClient _httpClient;

        public ApiRadioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ObservableCollection<Radio>> GetJsonFromGithub(string country)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{country}").ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var stationsList = JsonConvert.DeserializeObject<ObservableCollection<Radio>>(responseAsString);
                UpdateImageLink(stationsList, country.Substring(0, country.IndexOf(".")));
                #region Save to local
                Settings.SaveStationsSettings(country, JsonConvert.SerializeObject(stationsList));
                #endregion
                return stationsList;
            }
            catch(Exception ex)
            {
                var log = ex.Message;
            }
            return null;
        }
        private void UpdateImageLink(ObservableCollection<Radio> radios, string country)
        {
            foreach (var item in radios)
            {
                item.Image = $"https://michaelyakhnin.github.io/radio-app/src/assets/{country}/{item.Image}";
            }
        }

        public async Task<ObservableCollection<Radio>> GetStationsListLocalBackup(string country)
        {
            var list = new ObservableCollection<Radio>();
            using (var stream = await FileSystem.OpenAppPackageFileAsync(country))
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        var fileContents = reader.ReadToEnd();
                        var stationsList = JsonConvert.DeserializeObject<ObservableCollection<Radio>>(fileContents);
                        return stationsList;
                    }
                    catch (Exception ex)
                    {
                        var errpr = ex.Message;
                        return null;
                    }
                }
            }
        }

        public async Task<ObservableCollection<Radio>> GetStationsListLocal(string country)
        {
            try
            {
                var fileContents = Settings.GetStationsSettings(country);
                if (string.IsNullOrEmpty(fileContents))
                {
                    return await GetStationsListLocalBackup(country).ConfigureAwait(false);
                }
                var stationsList = JsonConvert.DeserializeObject<ObservableCollection<Radio>>(fileContents);
                return stationsList;
            }
            catch (Exception ex)
            {
                var errpr = ex.Message;
                return null;
            }
        }
    }
}
