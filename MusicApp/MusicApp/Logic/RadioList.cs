using MusicApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MusicApp.Logic
{
    public static class RadioList
    {
        public static async Task<ObservableCollection<Radio>> GetStationsListAsync(string country)
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
                    catch(Exception ex)
                    {
                        var errpr = ex.Message;
                        return null;
                    }
                    
                }
            }
        }
        public static async Task<ObservableCollection<Radio>> GetJsonFromGithub(string country)
        {

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync($"https://michaelyakhnin.github.io/radio-app/src/assets/mobile/{country}").ConfigureAwait(false);
                //string responseBody = await "http://10.100.102.9:5000/api/Books".GetStringAsync();
                response.EnsureSuccessStatusCode();

                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var stationsList = JsonConvert.DeserializeObject<ObservableCollection<Radio>>(responseBody);
                    return stationsList;
                }

            }
        }
    }
}
