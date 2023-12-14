using System;
using AutoFixture;
using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class EitherTests
{
    public class Match : EitherTests
    {
        public Match(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void Left_ShouldMatch()
        {
            // arrange
            var sut = Either<int, string>.Left(Fixture.Create<int>());

            // act
            var result = sut.Match(
                left: _ => State.Left,
                right: _ => State.Right);

            // assert
            result.Should().Be(State.Left);
        }

        [Fact]
        public void Left_ShouldNotCallRightFunc()
        {
            // arrange
            var sut = Either<int, string>.Left(Fixture.Create<int>());

            // act
            Action match = () => sut.Match(
                left: _ => State.Left,
                right: _ => throw new InvalidOperationException());

            // assert
            match.Should().NotThrow(because: "matching 'left' should not call 'right' function");
        }

        [Fact]
        public void Right_ShouldMatch()
        {
            // arrange
            var sut = Either<int, string>.Right(Fixture.Create<string>());

            // act
            var result = sut.Match(
                left: _ => State.Left,
                right: _ => State.Right);

            // assert
            result.Should().Be(State.Right);
        }

        [Fact]
        public void Right_ShouldNotCallLeftFunc()
        {
            // arrange
            var sut = Either<int, string>.Right(Fixture.Create<string>());

            // act
            Action match = () => sut.Match(
                left: _ => throw new InvalidOperationException(),
                right: _ => State.Right);

            // assert
            match.Should().NotThrow(because: "matching 'right' should not call 'left' function");
        }

        private enum State
        {
            Left,
            Right
        }
    }
}