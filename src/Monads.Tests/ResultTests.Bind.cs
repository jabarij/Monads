using System;
using AutoFixture;
using FluentAssertions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class ResultTests
{
    public class Bind : ResultTests
    {
        public Bind(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void Error_ShouldBeOriginalError()
        {
            // arrange
            string originalError = Fixture.Create<string>();
            var sut = Result<int, string>.Error(originalError);

            // act
            Result<string, string> result = sut.Bind<string>(
                binder: e => Result.Ok(e.ToString()));

            // assert
            result.Should().BeErrorOf(originalError);
        }

        [Fact]
        public void Error_ShouldNotCallBinder()
        {
            // arrange
            var sut = Result<int, string>.Error(Fixture.Create<string>());

            // act
            Action bind = () => sut.Bind<string>(
                binder: _ => throw new InvalidOperationException());

            // assert
            bind.Should().NotThrow(because: "binding 'error' should not call binder function");
        }

        [Fact]
        public void Ok_ShouldBind()
        {
            // arrange
            int value = Fixture.Create<int>();
            var sut = Result<int, string>.Ok(value);
            Func<int, Result<string, string>> binder = e => Result<string, string>.Ok(e.ToString());
            var expectedResult = binder(value);

            // act
            var result = sut.Bind(binder);

            // assert
            result.Should().Be(expectedResult);
        }
    }
}