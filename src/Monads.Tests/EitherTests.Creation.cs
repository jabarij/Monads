using AutoFixture;
using Monads.Extensions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class EitherTests
{
    public class Creation : EitherTests
    {
        public Creation(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void DefaultValue_ShouldBeLeft()
        {
            // arrange
            var sut = default(Either<int, string>);

            // act=
            // assert
            sut.Should().BeLeftVariant(because: "{0} is of default value and should be treated as 'left'", sut);
        }

        [Fact]
        public void ParameterlessConstructor_ShouldBeLeftOfDefaultValue()
        {
            // arrange
            var sut = new Either<int, string>();

            // act
            // assert
            sut.Should().BeLeftOf(default, because: "{0} is created using parameterless constructor and should be treated as 'left of default({1})'", sut, sut.GetLeftType());
        }

        [Fact]
        public void ConstructorWithValueOfLeftType_ShouldBeLeft()
        {
            // arrange
            int value = Fixture.Create<int>();
            var sut = new Either<int, string>(value);

            // act
            // assert
            sut.Should().BeLeftOf(value, because: "{0} is created using constructor with parameter of left type and should be treated as 'left of {1}'", sut, value);
        }

        [Fact]
        public void ConstructorWithValueOfRightType_ShouldBeRight()
        {
            // arrange
            var value = Fixture.Create<string>();
            var sut = new Either<int, string>(value);

            // act
            // assert
            sut.Should().BeRightOf(value, because: "{0} is created using constructor with parameter of right type and should be treated as 'right of {1}'", sut, value);
        }

        [Fact]
        public void DomesticLeft_ShouldBeLeft()
        {
            // arrange
            int value = Fixture.Create<int>();
            var sut = Either<int, int>.Left(value);

            // act
            // assert
            sut.Should().BeLeftOf(value, because: "{0} is created using domestic Left() method and should be 'left of {1}'", sut, value);
        }

        [Fact]
        public void DomesticRight_ShouldBeRight()
        {
            // arrange
            int value = Fixture.Create<int>();
            var sut = Either<int, int>.Right(value);

            // act
            // assert
            sut.Should().BeRightOf(value, because: "{0} is created using domestic Right() method and should be 'right of {1}'", sut, value);
        }
    }
}