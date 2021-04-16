using AutoFixture;
using FluentAssertions;
using Monads.TestAbstractions;
using System;
using Xunit;

namespace Monads.DataOps.Tests
{
    partial class ClearableValueTests
    {
        public class Match : ClearableValueTests
        {
            public Match(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void NoAction_ShouldMatch()
            {
                // arrange
                var sut = ClearableValue<int>.NoAction();

                // act
                var result = sut.Match(
                    set: _ => State.Set,
                    clear: () => State.Clear,
                    noAction: () => State.NoAction);

                // assert
                result.Should().Be(State.NoAction);
            }

            [Fact]
            public void NoAction_ShouldNotCallSetOrClearFunc()
            {
                // arrange
                var sut = ClearableValue<int>.NoAction();

                // act
                Action match = () => sut.Match(
                    set: _ => throw new InvalidOperationException(),
                    clear: () => throw new InvalidOperationException(),
                    noAction: () => State.NoAction);

                // assert
                match.Should().NotThrow(because: "matching 'noAction' should not call neither 'set' nor 'clear' function");
            }

            [Fact]
            public void Clear_ShouldMatch()
            {
                // arrange
                var sut = ClearableValue<int>.Clear();

                // act
                var result = sut.Match(
                    set: _ => State.Set,
                    clear: () => State.Clear,
                    noAction: () => State.NoAction);

                // assert
                result.Should().Be(State.Clear);
            }

            [Fact]
            public void Clear_ShouldNotCallSetOrNoActionFunc()
            {
                // arrange
                var sut = ClearableValue<int>.Clear();

                // act
                Action match = () => sut.Match(
                    set: _ => throw new InvalidOperationException(),
                    clear: () => State.Clear,
                    noAction: () => throw new InvalidOperationException());

                // assert
                match.Should().NotThrow(because: "matching 'clear' should not call neither 'set' nor 'noAction' function");
            }

            [Fact]
            public void Set_ShouldMatch()
            {
                // arrange
                var sut = ClearableValue<int>.Set(Fixture.Create<int>());

                // act
                var result = sut.Match(
                    set: _ => State.Set,
                    clear: () => State.Clear,
                    noAction: () => State.NoAction);

                // assert
                result.Should().Be(State.Set);
            }

            [Fact]
            public void Set_ShouldNotCallClearOrNoActionFunc()
            {
                // arrange
                var sut = ClearableValue<int>.Set(Fixture.Create<int>());

                // act
                Action match = () => sut.Match(
                    set: _ => State.Set,
                    clear: () => throw new InvalidOperationException(),
                    noAction: () => throw new InvalidOperationException());

                // assert
                match.Should().NotThrow(because: "matching 'set' should not call neither 'clear' nor 'noAction' function");
            }

            private enum State
            {
                NoAction,
                Set,
                Clear
            }
        }
    }
}
