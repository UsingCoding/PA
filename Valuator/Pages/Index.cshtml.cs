﻿using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Valuator.Infrastructure.Storage;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {
            _logger.LogDebug(text);

            var id = Guid.NewGuid().ToString();

            var rankKey = "RANK-" + id;
            _storage.Save(rankKey, CalculateRank(text).ToString());

            var similarityKey = "SIMILARITY-" + id;
            _storage.Save(similarityKey, IsSimilarity(text) ? "1" : "0");
            
            var textKey = "TEXT-" + id;
            _storage.Save(textKey, text);

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
            var texts = _storage.GetAllTexts();
            return texts.Exists(value => value == text);
        }
    }
}
