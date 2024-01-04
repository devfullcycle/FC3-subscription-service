namespace Subscription.Application.Abstractions.Handlers
{
    public class Input : IInput
    {
        public Input(Guid? guid)
        {
            CreateCorrelation(guid);
        }
        public string Host { get; private set; }
        public Guid CorrelationId { get; private set; }

        public void CreateCorrelation(Guid? correlationId = null)
        {
            CorrelationId = correlationId ?? Guid.NewGuid();
        }

        public void AddHostUrl(string url) => Host = url;
        public string GetHostUrl() => Host;
    }
}
