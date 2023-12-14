using System;
using AutoFixture;
using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class MaybeTests
{
    public class Match : MaybeTests
    {
        public Match(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void None_ShouldMatch()
        {
            // arrange
            var sut = Maybe<int>.None();

            // act
            var result = sut.Match(
                some: _ => State.Some,
                none: () => State.None);

            // assert
            result.Should().Be(State.None);
        }

        [Fact]
        public void None_ShouldNotCallSomeFunc()
        {
            // arrange
            var sut = Maybe<int>.None();

            // act
            Action match = () => sut.Match(
                some: _ => throw new InvalidOperationException(),
                none: () => State.None);

            // assert
            match.Should().NotThrow(because: "matching 'none' should not call 'some' function");
        }

        [Fact]
        public void Some_ShouldMatch()
        {
            // arrange
            var sut = Maybe<int>.Some(Fixture.Create<int>());

            // act
            var result = sut.Match(
                some: _ => State.Some,
                none: () => State.None);

            // assert
            result.Should().Be(State.Some);
        }

        [Fact]
        public void Some_ShouldNotCallNoneFunc()
        {
            // arrange
            var sut = Maybe<int>.Some(Fixture.Create<int>());

            // act
            Action match = () => sut.Match(
                some: _ => State.Some,
                none: () => throw new InvalidOperationException());

            // assert
            match.Should().NotThrow(because: "matching 'some' should not call 'none' function");
        }

        private enum State
        {
            None,
            Some
        }
    }
}