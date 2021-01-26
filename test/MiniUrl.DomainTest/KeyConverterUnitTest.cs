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
            IKeyConverter keyConverter = new KeyConverter(6);

            string keyMin = keyConverter.Encode(0);

            Assert.Equal(6, keyMin.Length);
        }
    }
}
