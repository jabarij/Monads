using FluentAssertions;
using Monads.Extensions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using System;
using Xunit;

namespace Monads.Tests
{
    partial class ResultExtensionsTests
    {
        public class FlattenOfDifferentErrorTypes : ResultExtensionsTests
        {
            public FlattenOfDifferentErrorTypes(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void ShouldMatchOuterErrorTypeAsMostLeftOption()
            {
                // arrange
                int? value = null;
                const int minValue = 1;
                const int maxValue = 3;
                Func<int?, Result<int, ValueIsNull>> checkIsNotNull = e => CheckIsNotNull(e);
                Func<int, Result<int, ValueIsTooSmall<int>>> checkIsGreaterThanMinValue = e => CheckIsGreaterThan(e, minValue);
                Func<int, Result<int, ValueIsTooBig<int>>> checkIsLowerThanMaxValue = e => CheckIsLowerThan(e, maxValue);

                var expectedError = Either<Either<ValueIsNull, ValueIsTooSmall<int>>, ValueIsTooBig<int>>
                    .Left(Either<ValueIsNull, ValueIsTooSmall<int>>
                        .Left(new ValueIsNull()));

                var sut = checkIsNotNull(value)
                    .Map(checkIsGreaterThanMinValue)
                    .Map(e => e.Map(checkIsLowerThanMaxValue));

                // act
                var result = sut.Flatten().Flatten();

                // assert
                result.Should().BeErrorOf(expectedError);
            }

            [Fact]
            public void ShouldMatchFirstInnerErrorTypeAsFirstRightOption()
            {
                // arrange
                int? value = 0;
                const int minValue = 1;
                const int maxValue = 3;
                Func<int?, Result<int, ValueIsNull>> checkIsNotNull = e => CheckIsNotNull(e);
                Func<int, Result<int, ValueIsTooSmall<int>>> checkIsGreaterThanMinValue = e => CheckIsGreaterThan(e, minValue);
                Func<int, Result<int, ValueIsTooBig<int>>> checkIsLowerThanMaxValue = e => CheckIsLowerThan(e, maxValue);

                var expectedError = Either<Either<ValueIsNull, ValueIsTooSmall<int>>, ValueIsTooBig<int>>
                    .Left(Either<ValueIsNull, ValueIsTooSmall<int>>
                        .Right(new ValueIsTooSmall<int>(min: minValue, actual: value.Value)));

                var sut = checkIsNotNull(value)
                    .Map(checkIsGreaterThanMinValue)
                    .Map(e => e.Map(checkIsLowerThanMaxValue));

                // act
                var result = sut.Flatten().Flatten();

                // assert
                result.Should().BeErrorOf(expectedError);
            }

            [Fact]
            public void ShouldMatchSecondInnerErrorTypeAsMostRightOption()
            {
                // arrange
                int? value = 4;
                const int minValue = 1;
                const int maxValue = 3;
                Func<int?, Result<int, ValueIsNull>> checkIsNotNull = e => CheckIsNotNull(e);
                Func<int, Result<int, ValueIsTooSmall<int>>> checkIsGreaterThanMinValue = e => CheckIsGreaterThan(e, minValue);
                Func<int, Result<int, ValueIsTooBig<int>>> checkIsLowerThanMaxValue = e => CheckIsLowerThan(e, maxValue);

                var expectedError = Either<Either<ValueIsNull, ValueIsTooSmall<int>>, ValueIsTooBig<int>>
                    .Right(new ValueIsTooBig<int>(max: maxValue, actual: value.Value));

                var sut = checkIsNotNull(value)
                    .Map(checkIsGreaterThanMinValue)
                    .Map(e => e.Map(checkIsLowerThanMaxValue));

                // act
                var result = sut.Flatten().Flatten();

                // assert
                result.Should().BeErrorOf(expectedError);
            }

            [Fact]
            public void ShouldMatchOkTypeProperly()
            {
                // arrange
                int? value = 2;
                int minValue = 1;
                int maxValue = 3;
                Func<int?, Result<int, ValueIsNull>> checkIsNotNull = e => CheckIsNotNull(e);
                Func<int, Result<int, ValueIsTooSmall<int>>> checkIsGreaterThanMinValue = e => CheckIsGreaterThan(e, minValue);
                Func<int, Result<int, ValueIsTooBig<int>>> checkIsLowerThanMaxValue = e => CheckIsLowerThan(e, maxValue);

                var sut = checkIsNotNull(value)
                    .Map(checkIsGreaterThanMinValue)
                    .Map(e => e.Map(checkIsLowerThanMaxValue));

                // act
                var result = sut.Flatten().Flatten();

                // assert
                result.Should().BeOkOf(value.Value);
            }

            private Result<T, ValueIsNull> CheckIsNotNull<T>(T? value) where T : struct =>
                value.HasValue
                ? Result<T, ValueIsNull>.Ok(value.Value)
                : Result<T, ValueIsNull>.Error(new ValueIsNull());

            private Result<int, ValueIsTooSmall<int>> CheckIsGreaterThan(int value, int min) =>
                value > min
                ? Result<int, ValueIsTooSmall<int>>.Ok(value)
                : Result<int, ValueIsTooSmall<int>>.Error(new ValueIsTooSmall<int>(min, value));

            private Result<int, ValueIsTooBig<int>> CheckIsLowerThan(int value, int max) =>
                value < max
                ? Result<int, ValueIsTooBig<int>>.Ok(value)
                : Result<int, ValueIsTooBig<int>>.Error(new ValueIsTooBig<int>(max, value));

            private readonly struct ValueIsNull
            {
            }

            private readonly struct ValueIsTooSmall<T>
            {
                public ValueIsTooSmall(T min, T actual)
                {
                    Min = min;
                    Actual = actual;
                }

                public readonly T Min;
                public readonly T Actual;
            }

            private readonly struct ValueIsTooBig<T>
            {
                public ValueIsTooBig(T max, T actual)
                {
                    Max = max;
                    Actual = actual;
                }

                public readonly T Max;
                public readonly T Actual;
            }
        }
    }
}
