using FluentAssertions;
using Monads.TestAbstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Monads.DataOps.Json.Tests
{
    public class ClearableValueJsonConverterTests : TestsBase
    {
        public ClearableValueJsonConverterTests(TestFixture testFixture) : base(testFixture) { }

        [Theory]
        [MemberData(nameof(GetTestData), typeof(ClearableValueJsonConverterTests), nameof(GetSerializeTestData))]
        public void SerializeAndDeserialize_NoAction_ShouldBeIdempotent(ClearableValue<string> originalValue)
        {
            // arrange
            var converter = new ClearableValueJsonConverter<string>();

            // act
            string serializedValue1 = JsonConvert.SerializeObject(originalValue, converter);
            var deserializedValue1 = JsonConvert.DeserializeObject<ClearableValue<string>>(serializedValue1, converter);
            string serializedValue2 = JsonConvert.SerializeObject(deserializedValue1, converter);
            var deserializedValue2 = JsonConvert.DeserializeObject<ClearableValue<string>>(serializedValue2, converter);

            // assert
            deserializedValue1.Should().Be(originalValue);
            deserializedValue2.Should().Be(originalValue);

            deserializedValue2.Should().Be(deserializedValue1);
            serializedValue2.Should().Be(serializedValue1);
        }
        private static IEnumerable<ClearableValue<string>> GetSerializeTestData()
        {
            yield return ClearableValue<string>.NoAction();
            yield return ClearableValue<string>.Clear();
            yield return ClearableValue<string>.Set(null);
            yield return ClearableValue<string>.Set(string.Empty);
            yield return ClearableValue<string>.Set(System.Guid.NewGuid().ToString());
        }
    }
}
