using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MKTL.WPF.Helpers
{
    public static class CryptoHelper
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("u$A_7!z#E+R5b@X8");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("t_9G!p$L@w*Z_1Qx");

        public static string DecryptConfig(byte[] cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}