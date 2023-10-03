namespace URLShorteningService.Validations
{
    public class URLShorteningValidation
    {
        private readonly int _shortUrlLength;
        public URLShorteningValidation(IConfiguration configuration)
        {
            _shortUrlLength = configuration.GetValue<int>("shortUrlLength");                
        }
        public bool IsShortUrlValid(string shortUrl) => (! string.IsNullOrEmpty(shortUrl) &&  shortUrl.Length == _shortUrlLength);       
    }
}