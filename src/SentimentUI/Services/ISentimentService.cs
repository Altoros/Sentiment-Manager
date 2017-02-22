using System.Threading.Tasks;

namespace SentimentUI.Services
{
    public interface ISentimentService
    {
        Task<float> GetSentiment(string message);
    }
}