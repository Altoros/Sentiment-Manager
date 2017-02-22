using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SentimentUI.Services;

namespace SentimentUI.Controllers
{
    [Route("api/[controller]")]
    public class SentimentController : Controller
    {
        private readonly IDistributedCache _cache;

        public SentimentController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public string Echo(string message)
        {
            return message;
        }

        [HttpGet]
        public Task<float> Get(string message, [FromServices] ISentimentService sentimentService)
        {
            var cacheKey = GetHash(message);
            var value = _cache.Get(cacheKey);
            if (value != null)
            {
                var data = Encoding.UTF8.GetString(value);
                if (!string.IsNullOrEmpty(data))
                    return Task.FromResult(float.Parse(data));
            }

            return sentimentService.GetSentiment(message);
        }

        private string GetHash(string message)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(message);

            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("X2"));

            return sb.ToString();
        }
    }
}