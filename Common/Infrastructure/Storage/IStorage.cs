using System.Collections.Generic;

namespace Common.Infrastructure.Storage
{
    public interface IStorage
    {
        public const string SegmentIdRu = "RU";
        public const string SegmentIdEu = "EU";
        public const string SegmentIdOther = "OTHER";
        
        public void Save(string shardKey, string key, string value);

        public void SaveNewShardId(string shardKey, string segmentId);
        public string Get(string shardKey, string key);
        public List<string> GetAllTexts();
    }
}