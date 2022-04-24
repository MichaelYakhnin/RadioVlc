using MusicApp.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace MusicApp.Services
{
    public interface IRadioService
    {
        Task<ObservableCollection<Radio>> GetJsonFromGithub(string country);
        Task<ObservableCollection<Radio>> GetStationsListLocal(string country);
    }
}
