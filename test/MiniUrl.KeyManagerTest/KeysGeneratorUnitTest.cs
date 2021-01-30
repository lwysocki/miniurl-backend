using System;
using Xunit;

using MiniUrl.KeyManager.Domain;

namespace MiniUrl.KeyManagerTest
{
    public class KeysGeneratorUnitTest
    {
        [Fact]
        public void KeysGeneratorShouldGenerateCorrectNumberOfKeys()
        {
            KeysGenerator keyGen = new KeysGenerator();
            int maxKey = 1000;
            int step = 100;
            int expectedKeysCount = maxKey / step;

            var keys = keyGen.Generate(maxKey, step);

            Assert.Equal(expectedKeysCount, keys.Count);
        }
    }
}
