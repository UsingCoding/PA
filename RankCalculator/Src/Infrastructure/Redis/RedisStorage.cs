using System;
using System.Collections.Generic;
using RankCalculator.App.Configuration;
using RankCalculator.Infrastructure.Storage;
using StackExchange.Redis;

namespace RankCalculator.Infrastructure.Redis
{
    public class RedisStorage : IStorage
    {
        private readonly IDatabase _db;
        
        public RedisStorage(IConfigurationProvider configurationProvider)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configurationProvider.RedisHost());
            _db = redis.GetDatabase();
        }

        public void Save(string key, string value)
        {
            if (key == null || value == null) return;
            
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