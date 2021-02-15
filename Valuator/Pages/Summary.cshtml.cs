using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Infrastructure.Storage;

namespace Valuator.Pages
{
    public class SummaryModel : PageModel
    {
        private const string RankPrefix = "RANK-";
        private const string SimilarityPrefix = "SIMILARITY-";
        
        private readonly ILogger<SummaryModel> _logger;
        private readonly IStorage _storage;

        public SummaryModel(ILogger<SummaryModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public string Rank { get; set; }
        public string Similarity { get; set; }

        public void OnGet(string id)
        {
            _logger.LogDebug(id);

            Rank = _storage.Get(RankPrefix + id);
            Similarity = _storage.Get(SimilarityPrefix + id);
        }
    }
}
