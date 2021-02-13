using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Common.Infrastructure.Repository;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly IKeyValueStorageClient _storageClient;

        public IndexModel(ILogger<IndexModel> logger, IKeyValueStorageClient storageClient)
        {
            _logger = logger;
            _storageClient = storageClient;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {
            _logger.LogDebug(text);

            var id = Guid.NewGuid().ToString();

            var textKey = "TEXT-" + id;
            _storageClient.Save(textKey, text);

            var rankKey = "RANK-" + id;
            _storageClient.Save(rankKey, CalculateRank(text).ToString());

            var similarityKey = "SIMILARITY-" + id;
            _storageClient.Save(similarityKey, IsSimilarity(text) ? "1" : "0");

            return Redirect($"summary?id={id}");
        }

        private static double CalculateRank(string text)
        {
            var regexp = new Regex(@"[A-Z,a-z,А-Я,а-я]");
            var nonAlphabetCharsCount = 0;

            foreach (var ch in text)
            {
                if (!regexp.IsMatch(ch.ToString()))
                {
                    nonAlphabetCharsCount++;
                }
            }

            if (nonAlphabetCharsCount == 0)
            {
                return 0;
            }

            return Math.Round(nonAlphabetCharsCount / (double) text.Length, 2);
        }

        private bool IsSimilarity(string text)
        {
            return true;
        }
    }
}
