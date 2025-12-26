using System.Net.Http;
using Newtonsoft.Json.Linq;
using MKTL.WPF.Helpers;

namespace MKTL.WPF.Services.Core
{
    public class ConfigService
    {
        private JObject _config;
        private const string Url = "";

        public async Task LoadAsync()
        {
            using var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(Url);
            var json = CryptoHelper.DecryptConfig(bytes);
            _config = JObject.Parse(json);
        }

        public string Get(string section, string key) => _config?[section]?[key]?.ToString();
    }
}