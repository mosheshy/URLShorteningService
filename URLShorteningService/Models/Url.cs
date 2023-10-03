using MongoDB.Bson;

namespace URLShorteningService.Models
{
    public class Url
    {
       public string _id { get; set; }
       public Uri Uri { get; set; }
    }
}