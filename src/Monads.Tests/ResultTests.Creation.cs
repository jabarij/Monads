using AutoFixture;
using FluentAssertions;
using Monads.Extensions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class ResultTests
{
    public class Creation : ResultTests
    {
        public Creation(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void DefaultValue_ShouldBeErrorOfDefaultValue()
        {
            // arrange
            var sut = default(Result<int, string>);

            // act=
            // assert
            sut.Should().BeErrorOf(default, because: "{0} is of default value and should be 'error of default({1})'", sut, sut.GetErrorType());
        }

        [Fact]
        public void ParameterlessConstructor_ShouldBeErrorOfDefaultValue()
        {
            // arrange
            var sut = new Result<int, string>();

            // act
            // assert
            sut.Should().BeErrorOf(default, because: "{0} is created using parameterless constructor and should be 'error of default({1})'", sut, sut.GetErrorType());
        }

        [Fact]
        public void ConstructorWithValueOfOkType_ShouldBeOk()
        {
            // arrange
            int value = Fixture.Create<int>();
            var sut = new Result<int, string>(value);

            // act
            // assert
            sut.Should().BeOkOf(value, because: "{0} is created using constructor with parameter of ok type and should be treated as 'ok of {1}'", sut, value);
        }

        [Fact]
        public void ConstructorWithValueOfErrorType_ShouldBeError()
        {
            // arrange
            var value = Fixture.Create<string>();
            var sut = new Result<int, string>(value);

            // act
            // assert
            sut.Should().BeErrorOf(value, because: "{0} is created using constructor with parameter of error type and should be treated as 'error of {1}'", sut, value);
        }

        [Fact]
        public void DomesticOk_ShouldBeOkResult()
        {
            // arrange
            var value = Fixture.Create<string>();
            var sut = Result<string, string>.Ok(value);

            // act
            // assert
            sut.Should().BeOkOf(value, because: "{0} is created using domestic Ok() method and should be treated as 'ok of {1}'", sut, value);
        }

        [Fact]
        public void DomesticError_ShouldBeErrorResult()
        {
            // arrange
            var value = Fixture.Create<string>();
            var sut = Result<string, string>.Error(value);

            // act
            // assert
            sut.Should().BeErrorOf(value, because: "{0} is created using domestic Error() method and should be treated as 'error of {1}'", sut, value);
        }

        [Fact]
        public void StaticOk_ShouldBeOk()
        {
            // arrange
            var value = Fixture.Create<string>();
            var sut = Result.Ok(value);

            // act
            // assert
            sut.Result.Should().Be(value, because: "{0} is created using static Error<T>() method and should be treated as 'error of {1}'", sut, value);
        }

        [Fact]
        public void StaticError_ShouldBeError()
        {
            // arrange
            var value = Fixture.Create<string>();
            var sut = Result.Error(value);

            // act
            // assert
            sut.Result.Should().Be(value, because: "{0} is created using static Error<T>() method and should be treated as 'error of {1}'", sut, value);
        }
    }
}