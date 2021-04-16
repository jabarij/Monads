using AutoFixture;
using FluentAssertions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using System;
using Xunit;

namespace Monads.Tests
{
    partial class MaybeTests
    {
        public class Map : MaybeTests
        {
            public Map(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void None_ShouldMapToNone()
            {
                // arrange
                var sut = Maybe<int>.None();

                // act
                var result = sut.Map(e => e.ToString());

                // assert
                result.Should().BeNoneVariant();
            }

            [Fact]
            public void Some_ShouldMapToSome()
            {
                // arrange
                int originalValue = Fixture.Create<int>();
                var sut = Maybe<int>.Some(originalValue);
                Func<int, string> map = e => e.ToString();
                string expectedValue = map(originalValue);

                // act
                var result = sut.Map(map);

                // assert
                result.Should().BeSomeOf(expectedValue);
            }

            [Fact]
            public void None_ShouldNotCallMapper()
            {
                // arrange
                var sut = Maybe<int>.None();

                // act
                Action map = () => sut.Map<int>(_ => throw new InvalidOperationException());

                // assert
                map.Should().NotThrow(because: "mapping 'none' should not call mapper function");
            }
        }
    }
}
