using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Common.App.Event;
using Valuator.Common.App.Service;
using Valuator.Infrastructure.Storage;

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

        public IActionResult OnPost(string text)
        {
            _logger.LogDebug(text);

            var id = Guid.NewGuid().ToString();
            
            var textId = "TEXT-" + id;
            _storage.Save(textId, text);

            _calculateRankSchedulerService.PostCalculateRankMessage(text, id);

            _similarityCalculationService.CalculateSimilarity(text, id);

            return Redirect($"summary?id={id}");
        }

        private bool IsSimilarity(string text)
        {
            var texts = _storage.GetAllTexts();
            return texts.Exists(value => value == text);
        }
    }
}
