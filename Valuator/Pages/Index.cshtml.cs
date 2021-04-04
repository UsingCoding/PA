using System;
using Common.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Common.App.Service;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;
        private readonly CalculateRankSchedulerService _calculateRankSchedulerService;
        private readonly SimilarityCalculationService _similarityCalculationService;

        public IndexModel(
            ILogger<IndexModel> logger, 
            IStorage storage, 
            CalculateRankSchedulerService calculateRankSchedulerService, 
            SimilarityCalculationService similarityCalculationService
        )
        {
            _logger = logger;
            _storage = storage;
            _calculateRankSchedulerService = calculateRankSchedulerService;
            _similarityCalculationService = similarityCalculationService;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text, string country)
        {
            _logger.LogDebug(text);

            var id = Guid.NewGuid().ToString();

            var segmentId = GetSegmentIdByCountry(country);
            _logger.LogInformation("LOOKUP: {0}, {1}", id, segmentId);

            _storage.SaveNewShardId(id, segmentId);
            
            var textId = "TEXT-" + id;

            _calculateRankSchedulerService.PostCalculateRankMessage(id, textId);

            _similarityCalculationService.CalculateSimilarity(text, id, textId);
            
            _storage.Save(id, textId, text);

            return Redirect($"summary?id={id}");
        }

        private static string GetSegmentIdByCountry(string country)
        {
            switch (country)
            {
                case "Russia":
                    return IStorage.SegmentIdRu;
                case "France":
                case "Germany":
                    return IStorage.SegmentIdEu;
                case "USA":
                case "India":
                    return IStorage.SegmentIdOther;
            }

            throw new ArgumentException("Country " + country + " doesn't support");
        }
    }
}
