namespace MiniUrl.KeyManager.Domain.Models
{
    public class Key(long id)
    {
        public long Id { get; set; } = id;
        public KeyState State { get; set; } = KeyState.New;
    }
}
