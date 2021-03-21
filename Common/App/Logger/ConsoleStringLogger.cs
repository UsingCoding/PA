namespace Common.App.Logger
{
    public interface IStringLogger : ILogger<string>
    {
        
    }

    public class ConsoleStringLogger : ConsoleLogger<string>, IStringLogger
    {
        
    }
}