namespace MiniUrl.Association.Domain.Model
{
    public class Key
    {
        public long Id { get; set; }
        public KeyState State { get; set; }

        public Key(long id)
        {
            Id = id;
            State = KeyState.New;
        }
    }
}
