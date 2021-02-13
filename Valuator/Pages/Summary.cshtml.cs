using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Common.Infrastructure.Repository;

namespace Valuator.Pages
{
    public class SummaryModel : PageModel
    {
        private const string RankPrefix = "RANK-";
        private const string SimilarityPrefix = "SIMILARITY-";
        
        private readonly ILogger<SummaryModel> _logger;
        private readonly IKeyValueStorageClient _storageClient;

        public SummaryModel(ILogger<SummaryModel> logger, IKeyValueStorageClient storageClient)
        {
            _logger = logger;
            _storageClient = storageClient;
        }

        public string Rank { get; set; }
        public string Similarity { get; set; }

        public void OnGet(string id)
        {
            _logger.LogDebug(id);

            Rank = _storageClient.Get(RankPrefix + id);
            Similarity = _storageClient.Get(SimilarityPrefix + id);
        }
    }
}
