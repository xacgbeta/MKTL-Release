using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace MKTL.WPF.ViewModels
{
    public class DlcItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public partial class DlcFinderViewModel : ObservableObject
    {
        [ObservableProperty] private string _searchTerm;
        [ObservableProperty] private string _status;
        public ObservableCollection<DlcItem> DlcList { get; } = new();

        [RelayCommand]
        public async Task Search()
        {
            DlcList.Clear();
            Status = "Searching...";

            // 1. Search for AppID via Steam Web API
            using var client = new HttpClient();
            // Simplified logic: Assume user typed AppID for now. 
            
            string appId = SearchTerm; 

            // 2. Get DLCs
            try 
            {
                string url = $"https://store.steampowered.com/api/dlcforapp/?appid={appId}";
                string json = await client.GetStringAsync(url);
                var data = JObject.Parse(json);
                
                foreach (var dlc in data["dlc"])
                {
                    DlcList.Add(new DlcItem 
                    { 
                        Id = dlc["id"].ToString(), 
                        Name = dlc["name"].ToString() 
                    });
                }
                Status = $"Found {DlcList.Count} DLCs.";
            }
            catch
            {
                Status = "Error or No DLCs found.";
            }
        }
    }
}