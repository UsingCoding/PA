using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Common.App.Service;
using Valuator.Infrastructure.Storage;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;
        private readonly CalculateRankSchedulerService _service;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage, CalculateRankSchedulerService service)
        {
            _logger = logger;
            _storage = storage;
            _service = service;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {
            _logger.LogDebug(text);

            var id = Guid.NewGuid().ToString();

            var similarityKey = "SIMILARITY-" + id;
            _storage.Save(similarityKey, IsSimilarity(text) ? "1" : "0");
            
            _service.PostCalculateRankMessage(text, id);

            return Redirect($"summary?id={id}");
        }

        private bool IsSimilarity(string text)
        {
            var texts = _storage.GetAllTexts();
            return texts.Exists(value => value == text);
        }
    }
}
