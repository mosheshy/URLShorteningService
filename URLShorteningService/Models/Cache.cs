using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

namespace URLShorteningService.Models
{
    public class Cache
    {
        public Uri Url { get; set; }
        public DateTime ExpiredTime {  get; set; }
        public DateTime LastUsed { get; set;}        
    }
}