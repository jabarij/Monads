using System;
using AutoFixture;
using FluentAssertions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class MaybeTests
{
    public class Bind : MaybeTests
    {
        public Bind(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void None_ShouldBeNone()
        {
            // arrange
            var sut = Maybe<int>.None();

            // act
            var result = sut.Bind(
                binder: e => Maybe<string>.Some(e.ToString()));

            // assert
            result.Should().BeNoneVariant();
        }

        [Fact]
        public void None_ShouldNotCallBinder()
        {
            // arrange
            var sut = Maybe<int>.None();

            // act
            Action bind = () => sut.Bind<string>(
                binder: _ => throw new InvalidOperationException());

            // assert
            bind.Should().NotThrow(because: "binding 'none' should not call binder function");
        }

        [Fact]
        public void Some_ShouldBind()
        {
            // arrange
            int value = Fixture.Create<int>();
            var sut = Maybe<int>.Some(value);
            Func<int, Maybe<string>> binder = e => Maybe<string>.Some(e.ToString());
            var expectedResult = binder(value);


            // act
            var result = sut.Bind(binder);

            // assert
            result.Should().Be(expectedResult);
        }
    }
}