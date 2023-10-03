using Microsoft.AspNetCore.Mvc;
using URLShorteningService.CacheLayer;

namespace URLShorteningService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheURLShortening _cacheURLShortening;
        private readonly ILogger<URLShorteningController> _logger;
        public CacheController(ILogger<URLShorteningController> logger, ICacheURLShortening cacheURLShortening)
        {
            _cacheURLShortening = cacheURLShortening;            
        }
        [HttpGet("GetAllCache")]
        public async Task< IActionResult> GetAll() 
        {
            try
            {
                var cache = _cacheURLShortening.GetAllCache();
                return  Ok(cache);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
            }
            return BadRequest();
        }
        [HttpGet("Get/{key}")]
        public IActionResult Get(string key) 
        {
            try
            {
                var cache = _cacheURLShortening.Get(key);
                return Ok(cache);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete/{key}")]
        public IActionResult Delete(string key)
        {
            try
            {
                var b = _cacheURLShortening.RemoveCache(key);
                return Ok(b);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteAll")]
        public IActionResult DeleteAll()
        {
            try
            {
                _cacheURLShortening.ClenCache();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }       
    }
}