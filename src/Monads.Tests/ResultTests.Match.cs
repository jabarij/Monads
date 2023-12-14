using System;
using AutoFixture;
using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class ResultTests
{
    public class Match : ResultTests
    {
        public Match(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void Ok_ShouldMatch()
        {
            // arrange
            var sut = Result<int, string>.Ok(Fixture.Create<int>());

            // act
            var result = sut.Match(
                ok: _ => State.Ok,
                error: _ => State.Error);

            // assert
            result.Should().Be(State.Ok);
        }

        [Fact]
        public void Ok_ShouldNotCallErrorFunc()
        {
            // arrange
            var sut = Result<int, string>.Ok(Fixture.Create<int>());

            // act
            Action match = () => sut.Match(
                ok: _ => State.Ok,
                error: _ => throw new InvalidOperationException());

            // assert
            match.Should().NotThrow(because: "matching 'ok' should not call 'error' function");
        }

        [Fact]
        public void Error_ShouldMatch()
        {
            // arrange
            var sut = Result<int, string>.Error(Fixture.Create<string>());

            // act
            var result = sut.Match(
                ok: _ => State.Ok,
                error: _ => State.Error);

            // assert
            result.Should().Be(State.Error);
        }

        [Fact]
        public void Error_ShouldNotCallOkFunc()
        {
            // arrange
            var sut = Result<int, string>.Error(Fixture.Create<string>());

            // act
            Action match = () => sut.Match(
                ok: _ => throw new InvalidOperationException(),
                error: _ => State.Error);

            // assert
            match.Should().NotThrow(because: "matching 'error' should not call 'ok' function");
        }

        private enum State
        {
            Ok,
            Error
        }
    }
}