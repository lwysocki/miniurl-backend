namespace MiniUrl.KeyManager.Domain.Models
{
    public class KeysGeneratorConfiguration : Configuration
    {
        public KeysGeneratorConfiguration()
        {
            Key = "KeyManager." + this.GetType().Name;
        }
    }
}
