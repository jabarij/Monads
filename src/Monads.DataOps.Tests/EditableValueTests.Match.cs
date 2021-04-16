using AutoFixture;
using FluentAssertions;
using Monads.TestAbstractions;
using System;
using Xunit;

namespace Monads.DataOps.Tests
{
    partial class EditableValueTests
    {
        public class Match : EditableValueTests
        {
            public Match(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void NoAction_ShouldMatch()
            {
                // arrange
                var sut = EditableValue<int>.NoAction();

                // act
                var result = sut.Match(
                    update: _ => State.Update,
                    noAction: () => State.NoAction);

                // assert
                result.Should().Be(State.NoAction);
            }

            [Fact]
            public void NoAction_ShouldNotCallUpdateFunc()
            {
                // arrange
                var sut = EditableValue<int>.NoAction();

                // act
                Action match = () => sut.Match(
                    update: _ => throw new InvalidOperationException(),
                    noAction: () => State.NoAction);

                // assert
                match.Should().NotThrow(because: "matching 'noAction' should not call 'update' function");
            }

            [Fact]
            public void Update_ShouldMatch()
            {
                // arrange
                var sut = EditableValue<int>.Update(Fixture.Create<int>());

                // act
                var result = sut.Match(
                    update: _ => State.Update,
                    noAction: () => State.NoAction);

                // assert
                result.Should().Be(State.Update);
            }

            [Fact]
            public void Update_ShouldNotCallNoActionFunc()
            {
                // arrange
                var sut = EditableValue<int>.Update(Fixture.Create<int>());

                // act
                Action match = () => sut.Match(
                    update: _ => State.Update,
                    noAction: () => throw new InvalidOperationException());

                // assert
                match.Should().NotThrow(because: "matching 'update' should not call 'noAction' function");
            }

            private enum State
            {
                NoAction,
                Update
            }
        }
    }
}
