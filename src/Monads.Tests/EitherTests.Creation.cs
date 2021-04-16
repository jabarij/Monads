using AutoFixture;
using FluentAssertions;
using Monads.TestAbstractions;
using Monads.FluentAssertions;
using Xunit;

namespace Monads.Tests
{
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
            public void ParameterlessConstructor_ShouldBeLeft()
            {
                // arrange
                var sut = new Either<int, string>();

                // act
                // assert
                sut.Should().BeLeftVariant(because: "{0} is created using parameterless constructor and should be treated as 'left'", sut);
            }

            [Fact]
            public void ConstructorWithValueOfLeftType_ShouldBeLeft()
            {
                // arrange
                var sut = new Either<int, string>(Fixture.Create<int>());

                // act
                // assert
                sut.Should().BeLeftVariant(because: "{0} is created using constructor with parameter of left type and should be treated as 'left'", sut);
            }

            [Fact]
            public void ConstructorWithValueOfRightType_ShouldBeRight()
            {
                // arrange
                var sut = new Either<int, string>(Fixture.Create<string>());

                // act
                // assert
                sut.Should().BeRightVariant(because: "{0} is created using constructor with parameter of right type and should be treated as 'right'", sut);
            }

            [Fact]
            public void DomesticLeft_ShouldBeLeft()
            {
                // arrange
                var sut = Either<int, string>.Left(Fixture.Create<int>());

                // act
                // assert
                sut.Should().BeLeftVariant(because: "{0} is created using domestic Left() method and should be treated as 'left'", sut);
            }

            [Fact]
            public void DomesticRight_ShouldBeLeft()
            {
                // arrange
                var sut = Either<int, string>.Right(Fixture.Create<string>());

                // act
                // assert
                sut.Should().BeRightVariant(because: "{0} is created using domestic Right() method and should be treated as 'right'", sut);
            }
        }
    }
}
