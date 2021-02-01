using System;
using System.Collections.Generic;
using Xunit;

using MiniUrl.KeyManager.Domain;

namespace MiniUrl.KeyManagerTest
{
    public class KeysGeneratorUnitTest
    {
        [Fact]
        public void KeysGeneratorShouldGenerateCorrectKeys()
        {
            int maxKey = 1000;
            int step = 100;
            int expectedKeysCount = maxKey / step;
            KeysGenerator keysGenerator = new KeysGenerator(maxKey, step);

            var keys = keysGenerator.Generate();

            Assert.Equal(expectedKeysCount, keys.Count);
            Assert.Equal(new List<long> { 1, 101, 201, 301, 401, 501, 601, 701, 801, 901 }, keys);
        }

        [Fact]
        public void KeyGeneratorGeneratedKeysShouldDependOnIteration()
        {
            int maxKey = 1000;
            int step = 100;
            int initialIteration = 1;
            int lastIteration = step - 2;
            KeysGenerator keysGenerator = new KeysGenerator(maxKey, step, initialIteration);

            var keysSecondIteration = keysGenerator.Generate();
            var keysThirdIteration = keysGenerator.Generate();

            keysGenerator = new KeysGenerator(maxKey, step, lastIteration);

            var keysFromLastIteration = keysGenerator.Generate();

            Assert.Equal(new List<long> { 2, 102, 202, 302, 402, 502, 602, 702, 802, 902 }, keysSecondIteration);
            Assert.Equal(new List<long> { 3, 103, 203, 303, 403, 503, 603, 703, 803, 903 }, keysThirdIteration);
            Assert.Equal(new List<long> { 9, 199, 299, 399, 499, 599, 699, 799, 899, 999 }, keysFromLastIteration);
        }
    }
}
