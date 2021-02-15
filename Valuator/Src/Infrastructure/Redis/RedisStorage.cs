using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Valuator.Common.App.Configuration;
using Valuator.Infrastructure.Storage;

namespace Valuator.Infrastructure.Redis
{
    public class RedisStorage : IStorage
    {
        private readonly ILogger<RedisStorage> _logger;
        private readonly IDatabase _db;
        
        public RedisStorage(ILogger<RedisStorage> logger, IConfigurationProvider configurationProvider)
        {
            _logger = logger;
            
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationProvider.RedisHost());
            _db = redis.GetDatabase();
        }

        public void Save(string key, string value)
        {
            if (key == null || value == null) return;
            
            _logger.LogDebug("Save to redis -  key: {Key} value:{Value}", key, value);
            _db.StringSet(key, value);
        }

        public string Get(string key)
        {
            if (key == null) throw new InvalidOperationException("Trying to get null from redis");
            
            return _db.StringGet(key);
        }

        public List<string> GetAllTexts()
        {
            var textsKeys = (RedisResult[])_db.Execute("keys", "TEXT-*");
            var texts = new List<string>();
            foreach (RedisResult key in textsKeys)
            {
                texts.Add(Get(key.ToString()));
            }
            return texts;
        }
    }
}