namespace Valuator.Common.Infrastructure.Repository
{
    public interface IKeyValueStorageClient
    {
        public void Save(string key, string value);
        public string Get(string key);
    }
}