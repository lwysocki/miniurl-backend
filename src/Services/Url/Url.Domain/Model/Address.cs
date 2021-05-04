namespace MiniUrl.Url.Domain.Model
{
    public class Address
    {
        public long Id { get; set; }
        public string Url { get; set; }

        public Address(long id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}
