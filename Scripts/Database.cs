using System.Text;
using System.Security.Cryptography;

namespace ZlabGrade.Scripts
{
    static class Database
    {
        public const string loginString = "server=sql7.freesqldatabase.com;user=sql7781252;password=ZdVzchgtJ7;database=sql7781252;charset=utf8mb4;";

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