using System;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Valuator.Common.App.Configuration;
using Valuator.Common.Infrastructure.Repository;

namespace Valuator.Infrastructure.Redis
{
    public class RedisClient : IKeyValueStorageClient
    {
        private readonly ILogger<RedisClient> _logger;
        private readonly IDatabase _db;
        
        public RedisClient(ILogger<RedisClient> logger, IConfigurationProvider configurationProvider)
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
    }
}