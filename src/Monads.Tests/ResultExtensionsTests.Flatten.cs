using FluentAssertions;
using Monads.Extensions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests
{
    partial class ResultExtensionsTests
    {
        public class Flatten : ResultExtensionsTests
        {
            public Flatten(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void ShouldMatchFirstErrorTypeProperly()
            {
                // arrange
                int? value = null;
                var result = CheckIsNotNull(value)
                    .Map(e => CheckIsGreaterThan(e, 1))
                    .Flatten();
                var expectedError = Either<ValueIsNull, ValueIsOutOfRange>.Left(new ValueIsNull());

                // act
                // assert
                result.Should().BeErrorOf(expectedError);
            }

            [Fact]
            public void ShouldMatchSecondErrorTypeProperly()
            {
                // arrange
                int? value = 0;
                var result = CheckIsNotNull(value)
                    .Map(e => CheckIsGreaterThan(e, 1))
                    .Flatten();
                var expectedError = Either<ValueIsNull, ValueIsOutOfRange>.Right(new ValueIsOutOfRange());

                // act
                // assert
                result.Should().BeErrorOf(expectedError);
            }

            [Fact]
            public void ShouldMatchOkTypeProperly()
            {
                // arrange
                int? value = 2;
                var result = CheckIsNotNull(value)
                    .Map(e => CheckIsGreaterThan(e, 1))
                    .Flatten();

                // act
                // assert
                result.Should().BeOkOf(value.Value);
            }

            private Result<T, ValueIsNull> CheckIsNotNull<T>(T? value) where T : struct =>
                value.HasValue
                ? Result<T, ValueIsNull>.Ok(value.Value)
                : Result<T, ValueIsNull>.Error(new ValueIsNull());

            private Result<int, ValueIsOutOfRange> CheckIsGreaterThan(int value, int min) =>
                value > min
                ? Result<int, ValueIsOutOfRange>.Ok(value)
                : Result<int, ValueIsOutOfRange>.Error(new ValueIsOutOfRange());

            private readonly struct ValueIsNull
            {
            }

            private readonly struct ValueIsOutOfRange
            {
            }
        }
    }
}
