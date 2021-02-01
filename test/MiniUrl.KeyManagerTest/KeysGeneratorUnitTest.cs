using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using MiniUrl.KeyManager.Domain;

namespace MiniUrl.KeyManagerTest
{
    public class KeysGeneratorUnitTest
    {
        [Fact]
        public void KeysGeneratorShouldGenerateCorrectKeys()
        {
            int keyLimit = 1000;
            int step = 100;
            int expectedKeysCount = keyLimit / step;
            KeysGenerator keysGenerator = new KeysGenerator(keyLimit, step);

            var keys = keysGenerator.Generate();

            Assert.Equal(expectedKeysCount, keys.Count);
            Assert.Equal(new List<long> { 1, 101, 201, 301, 401, 501, 601, 701, 801, 901 }, keys);
        }

        [Fact]
        public void KeyGeneratorGeneratedKeysShouldDependOnIterationGiven()
        {
            int keyLimit = 1000;
            int step = 100;
            int initialIteration = 1;
            KeysGenerator keysGenerator = new KeysGenerator(keyLimit, step, initialIteration);

            var keysSecondIteration = keysGenerator.Generate();

            Assert.Equal(new List<long> { 2, 102, 202, 302, 402, 502, 602, 702, 802, 902 }, keysSecondIteration);
        }

        [Fact]
        public void KeyGeneratorShouldGenerateFullRangeOfKeys()
        {
            int keyLimit = 100;
            int step = 10;
            int iterationCount = step;
            KeysGenerator keysGenerator = new KeysGenerator(keyLimit, step);

            long[] expectedKeys = new long[keyLimit - 1];
            long[] generatedKeys = new long[keyLimit - 1];

            for (int i = 1; i < keyLimit; i++)
                expectedKeys[i - 1] = i;

            while (iterationCount > 0)
            {
                iterationCount--;
                var keys = keysGenerator.Generate();
                int i = -1;

                foreach (var key in keys)
                {
                    generatedKeys[keysGenerator.Iteration + i] = key;
                    i += step;
                };
            }

            Assert.Equal(expectedKeys, generatedKeys);
        }
    }
}
