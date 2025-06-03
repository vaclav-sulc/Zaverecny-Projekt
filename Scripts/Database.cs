using System.Text;
using System.Security.Cryptography;

namespace ZlabGrade.Scripts
{
    static class Database
    {
        public const string loginString = "server=sqlskola.cps4c6aqi9mb.eu-central-1.rds.amazonaws.com;user=admin;password=adamSkola1#;database=ŽlabGrade;charset=utf8mb4;";

        public static string GetSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] hash = SHA256.HashData(buffer);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}