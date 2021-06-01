using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Services;
using Npgsql;
using Polly;
using Polly.Retry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeyManagerContextSeed
    {
        public async Task SeedAsync(KeysManagerContext context, IKeysGeneratorService keysGenerator, ILogger<KeyManagerContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(KeyManagerContextSeed));

            await policy.ExecuteAsync(async () =>
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
                    var keyEntities = keys.Select(k => new Key(k));

                    await context.Keys.AddRangeAsync(keyEntities);
                    await context.SaveChangesAsync();

                    configuration.Value = keysGenerator.SettingsJson;
                    context.KeyGeneratorConfigurations.Update(configuration);

                    await context.SaveChangesAsync();
                }
            });
        }

        private static AsyncRetryPolicy CreatePolicy(ILogger<KeyManagerContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<NpgsqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
