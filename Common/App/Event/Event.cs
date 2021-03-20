using System;

namespace Common.App.Event
{
    public class Event
    {
        public Event(string type, string payload)
        {
            Type = type;
            Payload = payload;
        }

        public string Type { get; }
        public string Payload { get; }
        
        public class UnmasrhallingFailedException : Exception
        {
            
        }
    }
}