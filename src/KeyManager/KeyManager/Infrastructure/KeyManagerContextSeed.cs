using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager.Domain;
using MiniUrl.KeyManager.Domain.Models;
using Npgsql;
using Polly;
using Polly.Retry;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeyManagerContextSeed
    {
        public async Task SeedAsync(KeyManagerContext context, IKeysGenerator keysGenerator, ILogger<KeyManagerContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(KeyManagerContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                KeyManagerConfiguration configuration = new KeyManagerConfiguration() { Value = keysGenerator.ConfigurationJson };

                if (!context.KeyManagerConfigurations.Any())
                {
                    await context.KeyManagerConfigurations.AddAsync(configuration);
                    await context.SaveChangesAsync();
                }
                else
                {
                    configuration = context.KeyManagerConfigurations.SingleOrDefault();
                    keysGenerator.ConfigurationJson = configuration.Value;
                }

                var keys = keysGenerator.Generate();
                var keyEntities = keys.Select(k => new Key(k));
                bool keysAdded = false;

                if (!context.Keys.Any())
                {
                    await context.Keys.AddRangeAsync(keyEntities);
                    await context.SaveChangesAsync();
                    keysAdded = !keysAdded;
                }

                if (keysAdded)
                {
                    configuration.Value = keysGenerator.ConfigurationJson;
                    context.KeyManagerConfigurations.Update(configuration);
                    await context.SaveChangesAsync();
                }
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<KeyManagerContextSeed> logger, string prefix, int retries = 3)
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
