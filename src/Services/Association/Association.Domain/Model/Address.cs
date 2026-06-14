namespace MiniUrl.Association.Domain.Model
{
    public class Address(long id, string url)
    {
        public long Id { get; set; } = id;
        public string Url { get; set; } = url;
    }
}
