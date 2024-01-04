namespace Subscription.Application.Abstractions.Handlers
{
    public class Output<TOutput> where TOutput : class
    {
        public TOutput Data { get; private set; }
        public MetaData Meta { get; private set; } = new();

        public void AddResult(TOutput output) => Data = output;
        public void AddMessage(string Message) => Meta.AddMessage(Message);
        public void AddMessages(List<string> messages) => Meta.AddMessages(messages);
    }
}
