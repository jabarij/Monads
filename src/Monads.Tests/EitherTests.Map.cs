using System;
using AutoFixture;
using FluentAssertions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class EitherTests
{
    public class Map : EitherTests
    {
        public Map(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void Left_ShouldMapLeft()
        {
            // arrange
            Func<int, string> leftMapper = e => e.ToString();
            Func<double, string> rightMapper = e => e.ToString();

            int value = Fixture.Create<int>();
            var sut = Either<int, double>.Left(value);
            string expectedValue = leftMapper(value);

            // act
            var result = sut.Map(
                leftMapping: leftMapper,
                rightMapping: rightMapper);

            // assert
            result.Should().BeLeftOf(expectedValue);
        }

        [Fact]
        public void Right_ShouldMapRight()
        {
            // arrange
            Func<int, string> leftMapper = e => e.ToString();
            Func<double, string> rightMapper = e => e.ToString();

            double value = Fixture.Create<double>();
            var sut = Either<int, double>.Right(value);
            string expectedValue = rightMapper(value);

            // act
            var result = sut.Map(
                leftMapping: leftMapper,
                rightMapping: rightMapper);

            // assert
            result.Should().BeRightOf(expectedValue);
        }

        [Fact]
        public void Left_ShouldNotCallRightMapper()
        {
            // arrange
            var sut = Either<int, double>.Left(Fixture.Create<int>());

            // act
            Action map = () => sut.Map<int, double>(
                leftMapping: _ => _,
                rightMapping: _ => throw new InvalidOperationException());

            // assert
            map.Should().NotThrow(because: "mapping 'left' should not call 'right' mapper function");
        }

        [Fact]
        public void Right_ShouldNotCallLeftMapper()
        {
            // arrange
            var sut = Either<int, double>.Right(Fixture.Create<double>());

            // act
            Action map = () => sut.Map<int, double>(
                leftMapping: _ => throw new InvalidOperationException(),
                rightMapping: _ => _);

            // assert
            map.Should().NotThrow(because: "mapping 'right' should not call 'left' mapper function");
        }
    }
}