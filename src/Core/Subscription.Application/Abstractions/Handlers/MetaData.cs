namespace Subscription.Application.Abstractions.Handlers
{
    public class MetaData
    {
        public List<string> Messages { get; private set; } = new();
        public bool IsValid { get { return !Messages.Any(); } }
        public List<Links> Linked { get; private set; } = new();

        public int PageSize { get; private set; } = 1;
        public int Page { get; private set; } = 1;
        public int Total { get; private set; } = 0;


        public void AddLink(string route) => Linked.Add(Links.AddLink(route));
        public void AddMessage(string message) => Messages.Add(message); 
        public void AddMessages(List<string> messages) => Messages.AddRange(messages);
        public void AddPagination(int pageSize, int page, int total)
        {
            Page = pageSize;
            Total = total;
            Page = page;
        }
    }
}
