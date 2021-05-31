using FluentAssertions;
using Monads.TestAbstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Monads.DataOps.Json.Tests
{
    public class EditableValueJsonConverterTests : TestsBase
    {
        public EditableValueJsonConverterTests(TestFixture testFixture) : base(testFixture) { }

        [Theory]
        [MemberData(nameof(GetTestData), typeof(EditableValueJsonConverterTests), nameof(GetSerializeTestData))]
        public void SerializeAndDeserialize_ShouldBeIdempotent(EditableValue<string> originalValue)
        {
            // arrange
            var converter = new EditableValueJsonConverter<string>();

            // act
            string serializedValue1 = JsonConvert.SerializeObject(originalValue, converter);
            var deserializedValue1 = JsonConvert.DeserializeObject<EditableValue<string>>(serializedValue1, converter);
            string serializedValue2 = JsonConvert.SerializeObject(deserializedValue1, converter);
            var deserializedValue2 = JsonConvert.DeserializeObject<EditableValue<string>>(serializedValue2, converter);

            // assert
            deserializedValue1.Should().Be(originalValue);
            deserializedValue2.Should().Be(originalValue);

            deserializedValue2.Should().Be(deserializedValue1);
            serializedValue2.Should().Be(serializedValue1);
        }
        private static IEnumerable<EditableValue<string>> GetSerializeTestData()
        {
            yield return EditableValue<string>.NoAction();
            yield return EditableValue<string>.Update(null);
            yield return EditableValue<string>.Update(string.Empty);
            yield return EditableValue<string>.Update(System.Guid.NewGuid().ToString());
        }
    }
}
