using System;
using System.Collections.Generic;
using Common.Infrastructure.Storage;
using StackExchange.Redis;

namespace Common.Infrastructure.Redis
{
    public class RedisStorage : IStorage
    {
        public interface IConfig
        {
            public string RedisMainDbHost();
            public string RedisRuDbHost();
            public string RedistEuHost();
            public string RedistOtherHost();
        }
        
        private readonly IConnectionMultiplexer _mainConn;
        private readonly IConnectionMultiplexer _ruConn;
        private readonly IConnectionMultiplexer _euConn;
        private readonly IConnectionMultiplexer _otherConn;

        public RedisStorage(IConfig config)
        {
            _mainConn = ConnectionMultiplexer.Connect(config.RedisMainDbHost());
            _ruConn = ConnectionMultiplexer.Connect(config.RedisRuDbHost());
            _euConn = ConnectionMultiplexer.Connect(config.RedistEuHost());
            _otherConn = ConnectionMultiplexer.Connect(config.RedistOtherHost());
        }

        public void Save(string shardKey, string key, string value)
        {
            if (key == null || value == null) return;
            
            GetDatabase(shardKey).StringSet(key, value);
        }

        public void SaveNewShardId(string shardKey, string segmentId)
        {
            var db = _mainConn.GetDatabase();
            db.StringSet(shardKey, segmentId);
        }

        public string Get(string shardKey, string key)
        {
            if (key == null) throw new InvalidOperationException("Trying to get null from redis");
            
            return GetDatabase(shardKey).StringGet(key);
        }

        public List<string> GetAllTexts()
        {
            // var db = GetDatabase()
            // var textsKeys = (RedisResult[])db.Execute("keys", "TEXT-*");
            // var texts = new List<string>();
            // foreach (RedisResult key in textsKeys)
            // {
            //     texts.Add(Get(key.ToString()));
            // }
            // return texts;
            return new();
        }

        private IDatabase GetDatabase(string shardKey)
        {
            var db = _mainConn.GetDatabase();
            if (!db.KeyExists(shardKey))
            {
                throw new ArgumentException("Shard key " + shardKey + " doesn't exist");
            }
            var segmentId = db.StringGet(shardKey).ToString();
            return segmentId switch
            {
                IStorage.SegmentIdRu => _ruConn.GetDatabase(),
                IStorage.SegmentIdEu => _euConn.GetDatabase(),
                IStorage.SegmentIdOther => _otherConn.GetDatabase(),
                _ => throw new ArgumentException("Shard key " + shardKey + " doesn't exist")
            };
        }
    }
}