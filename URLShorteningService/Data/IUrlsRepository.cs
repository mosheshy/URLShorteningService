using URLShorteningService.Models;

namespace URLShorteningService.Data
{
    public interface IUrlsRepository
    {
        Task<Url> Creat(Url url);
        Task<Url> get(string id);
        List<Url> GetList();
        bool Remove(string id);
    }
}