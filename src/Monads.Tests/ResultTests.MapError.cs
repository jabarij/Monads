using AutoFixture;
using FluentAssertions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using System;
using Xunit;

namespace Monads.Tests
{
    partial class ResultTests
    {
        public class MapError : ResultTests
        {
            public MapError(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void Error_ShouldMap()
            {
                // arrange
                Func<double, string> errorMapper = e => e.ToString();

                int value = Fixture.Create<int>();
                var sut = Result<int, double>.Error(value);
                string expectedValue = errorMapper(value);

                // act
                var result = sut.MapError(errorMapper);

                // assert
                result.Should().BeErrorOf(expectedValue);
            }

            [Fact]
            public void Ok_ShouldNotCallMapper()
            {
                // arrange
                var sut = Result<int, string>.Ok(Fixture.Create<int>());

                // act
                Action map = () => sut.MapError<string>(_ => throw new InvalidOperationException());

                // assert
                map.Should().NotThrow(because: "mapping 'ok' should not call error mapper function");
            }
        }
    }
}
