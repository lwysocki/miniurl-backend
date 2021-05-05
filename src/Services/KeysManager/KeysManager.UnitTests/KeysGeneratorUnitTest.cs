using Microsoft.Extensions.Options;
using MiniUrl.KeyManager.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MiniUrl.KeyManager.UnitTests
{
    public class KeysGeneratorUnitTest
    {
        [Fact]
        public void KeysGeneratorShouldGenerateCorrectKeys()
        {
            var settings = new TestKeysGeneratorSettings();
            long expectedKeysCount = settings.Value.Limit / settings.Value.Step;
            IKeysGeneratorService keysGenerator = new KeysGeneratorService(settings);

            var keys = keysGenerator.Generate();

            Assert.Equal(expectedKeysCount, keys.Count);
            Assert.Equal(new List<long> { 1, 101, 201, 301, 401, 501, 601, 701, 801, 901 }, keys);
        }

        [Fact]
        public void KeyGeneratorGeneratedKeysShouldDependOnIterationGiven()
        {
            var settings = new TestKeysGeneratorSettings();
            KeysGeneratorService keysGenerator = new(settings);
            keysGenerator.Settings.Iteration = 1;

            var keysSecondIteration = keysGenerator.Generate();

            Assert.Equal(new List<long> { 2, 102, 202, 302, 402, 502, 602, 702, 802, 902 }, keysSecondIteration);
        }

        [Fact]
        public void KeyGeneratorShouldGenerateFullRangeOfKeys()
        {
            var settings = new TestKeysGeneratorSettings();
            int limit = (int)settings.Value.Limit;
            int step = (int)settings.Value.Step;
            int iterationCount = (int)settings.Value.Step;
            KeysGeneratorService keysGenerator = new(settings);

            long[] expectedKeys = new long[limit - 1];
            long[] generatedKeys = new long[limit - 1];

            for (int i = 1; i < limit; i++)
                expectedKeys[i - 1] = i;

            while (iterationCount > 0)
            {
                iterationCount--;
                var keys = keysGenerator.Generate();
                int i = -1;

                foreach (var key in keys)
                {
                    generatedKeys[keysGenerator.Settings.Iteration + i] = key;
                    i += step;
                };
            }

            Assert.Equal(expectedKeys, generatedKeys);
        }

        [Fact]
        public void KeyGeneratotShallNotGenerateConflictKeys()
        {
            var settings = new TestKeysGeneratorSettings();
            int iterationCount = (int)settings.Value.Step;
            IKeysGeneratorService keysGenerator = new KeysGeneratorService(settings);

            Dictionary<long, int> keyCount = new();

            while (iterationCount > 0)
            {
                iterationCount--;
                var keys = keysGenerator.Generate();

                foreach (var key in keys)
                {
                    if (!keyCount.ContainsKey(key))
                        keyCount.Add(key, 1);
                    else
                        keyCount[key]++;
                };
            }

            Assert.True(!keyCount.Values.Any(v => v != 1));
        }
    }

    public class TestKeysGeneratorSettings : IOptionsSnapshot<KeysGeneratorService.KeysGeneratorSettings>
    {
        public KeysGeneratorService.KeysGeneratorSettings Value => new()
        {
            Iteration = 0,
            Limit = 1000,
            Step = 100
        };

        public KeysGeneratorService.KeysGeneratorSettings Get(string name) => Value;
    }
}
