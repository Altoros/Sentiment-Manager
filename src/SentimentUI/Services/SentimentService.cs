using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Pivotal.Discovery.Client;

namespace SentimentUI.Services
{
    public class SentimentService : ISentimentService
    {
        private const string SERVICE_URL = "http://sentimentapi/api/sentiment";
        private readonly DiscoveryHttpClientHandler _handler;

        public SentimentService(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        public async Task<float> GetSentiment(string message)
        {
            var client = GetClient();
            return
                float.Parse(await client.GetStringAsync($"{SERVICE_URL}?&message={UrlEncoder.Default.Encode(message)}"));
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}