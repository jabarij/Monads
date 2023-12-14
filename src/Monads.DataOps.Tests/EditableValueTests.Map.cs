using System;
using AutoFixture;
using FluentAssertions;
using Monads.DataOps.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.DataOps.Tests;

partial class EditableValueTests
{
    public class Map : EditableValueTests
    {
        public Map(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void NoAction_ShouldMapToNoAction()
        {
            // arrange
            var sut = EditableValue<int>.NoAction();

            // act
            var result = sut.Map(e => e.ToString());

            // assert
            result.Should().BeNoActionVariant();
        }

        [Fact]
        public void Update_ShouldMapToUpdate()
        {
            // arrange
            int originalValue = Fixture.Create<int>();
            var sut = EditableValue<int>.Update(originalValue);
            Func<int, string> map = e => e.ToString();
            string expectedValue = map(originalValue);

            // act
            var result = sut.Map(map);

            // assert
            result.Should().BeUpdateOf(expectedValue);
        }

        [Fact]
        public void NoAction_ShouldNotCallMapper()
        {
            // arrange
            var sut = EditableValue<int>.NoAction();

            // act
            Action map = () => sut.Map<int>(_ => throw new InvalidOperationException());

            // assert
            map.Should().NotThrow(because: "mapping 'noAction' should not call mapper function");
        }
    }
}