using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeyManagerContextSeed
    {
        public async Task SeedAsync(KeysManagerContext context, IKeysGeneratorService keysGenerator, ILogger<KeyManagerContextSeed> logger)
        {
            KeysGeneratorConfiguration configuration = new() { Value = keysGenerator.SettingsJson };

            if (!context.KeyGeneratorConfigurations.Any())
            {
                await context.KeyGeneratorConfigurations.AddAsync(configuration);
                await context.SaveChangesAsync();
            }
            else
            {
                configuration = context.KeyGeneratorConfigurations.SingleOrDefault();
                keysGenerator.SettingsJson = configuration.Value;
            }

            if (!context.Keys.Any())
            {
                var keys = keysGenerator.Generate();
                var chunks = keys.Select(k => new Key(k)).Chunk(1000);

                logger.LogInformation("Keys generated: {count}", keys.Count);

                foreach (var chunk in chunks)
                {
                    await context.Keys.AddRangeAsync(chunk);
                    await context.SaveChangesAsync();
                }

                logger.LogInformation("Keys successfully generated and seeded in batches.");

                configuration.Value = keysGenerator.SettingsJson;
                context.KeyGeneratorConfigurations.Update(configuration);

                await context.SaveChangesAsync();
            }
        }
    }
}
