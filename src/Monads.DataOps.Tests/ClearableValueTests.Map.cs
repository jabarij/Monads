using System;
using AutoFixture;
using FluentAssertions;
using Monads.DataOps.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.DataOps.Tests;

partial class ClearableValueTests
{
    public class Map : ClearableValueTests
    {
        public Map(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void NoAction_ShouldMapToNoAction()
        {
            // arrange
            var sut = ClearableValue<int>.NoAction();

            // act
            var result = sut.Map(e => e.ToString());

            // assert
            result.Should().BeNoActionVariant();
        }

        [Fact]
        public void Clear_ShouldMapToClear()
        {
            // arrange
            var sut = ClearableValue<int>.Clear();

            // act
            var result = sut.Map(e => e.ToString());

            // assert
            result.Should().BeClearVariant();
        }

        [Fact]
        public void Set_ShouldMapToSet()
        {
            // arrange
            int originalValue = Fixture.Create<int>();
            var sut = ClearableValue<int>.Set(originalValue);
            Func<int, string> map = e => e.ToString();
            string expectedValue = map(originalValue);

            // act
            var result = sut.Map(map);

            // assert
            result.Should().BeSetOf(expectedValue);
        }

        [Fact]
        public void NoAction_ShouldNotCallMapper()
        {
            // arrange
            var sut = ClearableValue<int>.NoAction();

            // act
            Action map = () => sut.Map<int>(_ => throw new InvalidOperationException());

            // assert
            map.Should().NotThrow(because: "mapping 'noAction' should not call mapper function");
        }
    }
}