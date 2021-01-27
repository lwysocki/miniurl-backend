using System;
using Xunit;

using MiniUrl.Domain;

namespace MiniUrl.DomainTest
{
    public class KeyConverterUnitTest
    {
        [Fact]
        public void KeyEncodingShouldReturnsStringOfProperLength()
        {
            int keyLength = 6;
            long originalValue = 0;
            IKeyConverter keyConverter = new KeyConverter(keyLength);

            string keyMin = keyConverter.Encode(originalValue);

            Assert.Equal(keyLength, keyMin.Length);
        }

        [Fact]
        public void KeyDecodingShouldReturnOriginalNumber()
        {
            int keyLength = 6;
            long originalValue = 0;
            IKeyConverter keyConverter = new KeyConverter(keyLength);

            string key = keyConverter.Encode(originalValue);

            Assert.Equal(originalValue, keyConverter.Decode(key));
        }
    }
}
