using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace MiniUrl.Shared.Domain.UnitTests
{
    public class KeyConverterUnitTest
    {
        [Fact]
        public void KeyEncodingShouldReturnsStringOfProperLength()
        {
            var settings = new TestKeyConverterSettings();
            int keyLength = settings.Value.KeyOrder;
            IKeyConverter keyConverter = new KeyConverter(settings);

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
            var settings = new TestKeyConverterSettings();
            IKeyConverter keyConverter = new KeyConverter(settings);

            long originalValue = 0;
            string key = keyConverter.Encode(originalValue);

            Assert.Equal(originalValue, keyConverter.Decode(key));
        }
    }

    class TestKeyConverterSettings : IOptionsSnapshot<KeyConverter.KeyConverterSettings>
    {
        public KeyConverter.KeyConverterSettings Value => new()
        {
            KeyOrder = 6
        };

        public KeyConverter.KeyConverterSettings Get(string name) => Value;
    }
}
