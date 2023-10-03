using Microsoft.AspNetCore.Mvc;
using URLShorteningService.BL;
using URLShorteningService.CacheLayer;
using URLShorteningService.Validations;

namespace URLShorteningService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class URLShorteningController : ControllerBase
    {
        private readonly ILogger<URLShorteningController> _logger;
        private readonly IUrlBl _urlBl;
        private readonly ICacheURLShortening _cacheURLShortening;
        private readonly URLShorteningValidation _uRLShorteningValidation;
        public URLShorteningController(URLShorteningValidation uRLShorteningValidation, ILogger<URLShorteningController> logger, IUrlBl urlBl, ICacheURLShortening cacheURLShortening)
        {
            _logger = logger;
            _urlBl = urlBl;
            _cacheURLShortening = cacheURLShortening;
            _uRLShorteningValidation = uRLShorteningValidation;
        }

        [HttpPost("generateUrl")]
        public async Task<IActionResult> GenerateShortUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var newUrl = await _urlBl.GenerateShortUrl(uri);
                return Ok($"url : {newUrl.Uri} shortUrl : {newUrl._id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ObjectResult(500, ex.ToString());
            }
        }
        [HttpGet("extractShortUrl/{shortUrl}")]
        public async Task<IActionResult> ExtractShortUrl(string shortUrl)
        {
            try
            {
                if (!_uRLShorteningValidation.IsShortUrlValid(shortUrl))
                {
                    return BadRequest($"sort url not valid");
                }
                var cache = _cacheURLShortening.Get(shortUrl);
                if (cache.Value != null)
                {
                    return Ok(cache.Value.Url);
                }
                var url = await _urlBl.GetUrl(shortUrl);
                _cacheURLShortening.Add(shortUrl, url.Uri);
                return Ok(url.Uri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ObjectResult(500, ex.ToString());
            }
        }
        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectUrl(string shortUrl)
        {
            try
            {
                if (!_uRLShorteningValidation.IsShortUrlValid(shortUrl))
                {
                    return BadRequest($"sort url not valid");
                }
                var cache = _cacheURLShortening.Get(shortUrl);
                if (cache.Value != null)
                {
                    return Redirect(cache.Value.Url.ToString());
                }
                var url = await _urlBl.GetUrl(shortUrl);
                _cacheURLShortening.Add(shortUrl, url.Uri);
                return Redirect(url.Uri.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ObjectResult(500, ex.ToString());
            }
        } 
        private IActionResult ObjectResult(int statusCode, object value)
        {
            var result = new ObjectResult(new {  statusCode, currentDate = DateTime.Now, JsonResult = value });
            result.StatusCode = statusCode;
            return result;
        }
    }
}