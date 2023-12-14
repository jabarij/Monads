using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class EitherTests
{
    public class Equality : EitherTests
    {
        public Equality(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void LeftWithDefaultValueAndDefaultEither_ShouldBeEqual()
        {
            // arrange
            var right = Either<int, string>.Left(default);
            var @default = default(Either<int, string>);

            // act
            // assert
            right.Equals(@default).Should().BeTrue();
            @default.Equals(right).Should().BeTrue();
        }

        [Fact]
        public void LeftOfSameValues_ShouldBeEqual()
        {
            // arrange
            var left1 = Either<int, int>.Left(1);
            var left2 = Either<int, int>.Left(1);

            // act
            // assert
            left1.Equals(left2).Should().BeTrue();
            left2.Equals(left1).Should().BeTrue();
        }

        [Fact]
        public void LeftOfDifferentValues_ShouldNotBeEqual()
        {
            // arrange
            var left1 = Either<int, int>.Left(1);
            var left2 = Either<int, int>.Left(2);

            // act
            // assert
            left1.Equals(left2).Should().BeFalse();
            left2.Equals(left1).Should().BeFalse();
        }

        [Fact]
        public void RightOfSameValues_ShouldBeEqual()
        {
            // arrange
            var right1 = Either<int, int>.Right(1);
            var right2 = Either<int, int>.Right(1);

            // act
            // assert
            right1.Equals(right2).Should().BeTrue();
            right2.Equals(right1).Should().BeTrue();
        }

        [Fact]
        public void RightOfDifferentValues_ShouldNotBeEqual()
        {
            // arrange
            var right1 = Either<int, int>.Right(1);
            var right2 = Either<int, int>.Right(2);

            // act
            // assert
            right1.Equals(right2).Should().BeFalse();
            right2.Equals(right1).Should().BeFalse();
        }

        [Fact]

        public void LeftAndRightOfSameValues_ShouldNotBeEqual()
        {
            // arrange
            var left = Either<int, int>.Left(1);
            var right = Either<int, int>.Right(1);

            // act
            // assert
            left.Equals(right).Should().BeFalse();
            right.Equals(left).Should().BeFalse();
        }
    }
}