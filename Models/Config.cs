using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;

namespace MKTL.WPF.Services 
{
    public class ConfigService
    {
        private JObject _configData = new JObject(); // Initialize to empty
        private readonly byte[] _key = Encoding.UTF8.GetBytes("u$");
        private readonly byte[] _iv = Encoding.UTF8.GetBytes("tw");
        private const string ConfigUrl = "n";

        public async Task LoadConfigAsync()
        {
            using var client = new HttpClient();
            var encryptedBytes = await client.GetByteArrayAsync(ConfigUrl);
            var jsonString = DecryptStringFromBytes_Aes(encryptedBytes, _key, _iv);
            _configData = JObject.Parse(jsonString);
        }

        public string Get(string section, string key)
        {
            return _configData?[section]?[key]?.ToString() ?? "";
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Mode = CipherMode.CBC;

            using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}