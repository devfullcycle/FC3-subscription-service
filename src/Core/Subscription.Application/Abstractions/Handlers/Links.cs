namespace Subscription.Application.Abstractions.Handlers
{
    public class Links
    {
        public string HRef { get; private set; }
        public static Links AddLink(string href)
        {
            var link = new Links();
            link.HRef = href;
            return link;
        }
    }
}
