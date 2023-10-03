using System.Security.Cryptography;
using System.Text;

namespace URLShorteningService.Test
{
    public class URLShorteningServiceTestSources
    {
        private readonly Random _random;
        private const string _letterAndDigit = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


        public URLShorteningServiceTestSources()
        {
                _random = new Random();
        }

        private string GetRandomString(int length)
        {
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(_letterAndDigit[_random.Next(_letterAndDigit.Length)]);
            }
            return stringBuilder.ToString();
        }

        public string GenerateShortUrlString(string url, int shortUrlLength = 8)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(url));
                    var shortrl = new string(Convert.ToBase64String(bytes, 0, bytes.Length)
                    .Where(char.IsLetterOrDigit)
                    .Take(shortUrlLength)
                    .ToArray());

                    if (shortrl.Length == shortUrlLength)
                    {
                        return shortrl;
                    }
                    throw new Exception($"error {shortrl.Length}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} error ");
            }
        }
        public string GenerateUrlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("https://");
            sb.Append("test.com/");
            sb.Append(GetRandomString(20 + _random.Next(300)));
            return sb.ToString();
        }


    }
}
