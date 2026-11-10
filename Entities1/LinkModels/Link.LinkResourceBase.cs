namespace Entities1.LinkModels
{
    public partial class Link
    {
        public class LinkResourceBase
        {
            public LinkResourceBase()
            {

            }
            public List<Link> Links { get; set; } = new List<Link>();
        }

    }
}
