using System.IO;
using System.Text.RegularExpressions;

namespace MKTL.WPF.Services.Steam
{
    public class UpdateTogglerService
    {
        public void ToggleUpdate(string steamPath, string appId, bool enable)
        {
            string luaPath = Path.Combine(steamPath, "config", "stplug-in", $"{appId}.lua");
            
            if (!File.Exists(luaPath)) return;

            string content = File.ReadAllText(luaPath);
            string newContent;

            if (enable)
            {
                newContent = Regex.Replace(content, @"^--\s*(addappid)", "$1", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
            else
            {
                newContent = Regex.Replace(content, @"^(addappid)", "--$1", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }

            File.WriteAllText(luaPath, newContent);
        }
    }
}