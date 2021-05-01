using System;
using Xunit;

using MiniUrl.Shared.Domain;

namespace MiniUrl.Shared.Domain.UnitTests
{
    public class KeyConverterUnitTest
    {
        [Fact]
        public void KeyEncodingShouldReturnsStringOfProperLength()
        {
            int keyLength = 6;
            IKeyConverter keyConverter = new KeyConverter(keyLength);

            long originalMinValue = 0;
            long originalMaxValue = (long)(Math.Pow(keyConverter.AlphabetSize, keyLength) - Math.Pow(keyConverter.AlphabetSize, keyLength - 1)) - 1;
            string keyMin = keyConverter.Encode(originalMinValue);
            string keyMax = keyConverter.Encode(originalMaxValue);

            long exceededMaxValue = originalMaxValue + 1;
            string keyAboveMax = keyConverter.Encode(exceededMaxValue);

            Assert.Equal(keyLength, keyMin.Length);
            Assert.Equal(keyLength, keyMax.Length);
            Assert.True(keyAboveMax.Length > keyLength);
        }

        [Fact]
        public void KeyDecodingShouldReturnOriginalNumber()
        {
            int keyLength = 6;
            IKeyConverter keyConverter = new KeyConverter(keyLength);

            long originalValue = 0;
            string key = keyConverter.Encode(originalValue);

            Assert.Equal(originalValue, keyConverter.Decode(key));
        }
    }
}
