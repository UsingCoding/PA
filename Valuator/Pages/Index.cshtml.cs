using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
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

            string id = Guid.NewGuid().ToString();

            string textKey = "TEXT-" + id;
            //TODO: сохранить в БД text по ключу textKey

            string rankKey = "RANK-" + id;
            //TODO: посчитать rank и сохранить в БД по ключу rankKey

            string similarityKey = "SIMILARITY-" + id;
            //TODO: посчитать similarity и сохранить в БД по ключу similarityKey

            return Redirect($"summary?id={id}");
        }
    }
}
