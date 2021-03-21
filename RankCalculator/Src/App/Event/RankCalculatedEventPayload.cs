namespace RankCalculator.App.Event
{
    public class RankCalculatedEventPayload
    {
        public RankCalculatedEventPayload(string textId, double rank)
        {
            TextId = textId;
            Rank = rank;
        }

        public string TextId { get; }
        public double Rank { get; }
    }
}