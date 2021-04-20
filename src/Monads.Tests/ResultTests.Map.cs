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
        public class Map : ResultTests
        {
            public Map(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void Ok_ShouldMap()
            {
                // arrange
                Func<int, string> mapper = e => e.ToString();

                int value = Fixture.Create<int>();
                var sut = Result<int, double>.Ok(value);
                string expectedValue = mapper(value);

                // act
                var result = sut.Map(mapper);

                // assert
                result.Should().BeOkOf(expectedValue);
            }

            [Fact]
            public void Error_ShouldNotCallMapper()
            {
                // arrange
                var sut = Result<int, string>.Error(Fixture.Create<string>());

                // act
                Action map = () => sut.Map<int>(_ => throw new InvalidOperationException());

                // assert
                map.Should().NotThrow(because: "mapping 'error' should not call mapper function");
            }
        }
    }
}
