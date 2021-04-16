using AutoFixture;
using FluentAssertions;
using Monads.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests
{
    partial class MaybeTests
    {
        public class Creation : MaybeTests
        {
            public Creation(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void DefaultValue_ShouldBeNone()
            {
                // arrange
                var sut = default(Maybe<int>);

                // act=
                // assert
                sut.Should().BeNoneVariant(because: "{0} is of default value and should be treated as 'none'", sut);
            }

            [Fact]
            public void ParameterlessConstructor_ShouldBeNone()
            {
                // arrange
                var sut = new Maybe<int>();

                // act
                // assert
                sut.Should().BeNoneVariant(because: "{0} is created using parameterless constructor and should be treated as 'none'", sut);
            }

            [Fact]
            public void ConstructorWithValue_ShouldBeSome()
            {
                // arrange
                var sut = new Maybe<int>(Fixture.Create<int>());

                // act
                // assert
                sut.Should().BeSomeVariant(because: "{0} is created using constructor with value parameter and should be treated as 'some'", sut);
            }

            [Fact]
            public void DomesticNone_ShouldBeNone()
            {
                // arrange
                var sut = Maybe<int>.None();

                // act
                // assert
                sut.Should().BeNoneVariant(because: "{0} is created using domestic static None() method and should be treated as 'none'", sut);
            }

            [Fact]
            public void GenericNone_ShouldBeNone()
            {
                // arrange
                var sut = Maybe.None(Fixture.Create<int>());

                // act
                // assert
                sut.Should().BeNoneVariant(because: "{0} is created using generic None<T>() method and should be treated as 'none'", sut);
            }

            [Fact]
            public void DomesticSome_ShouldBeSome()
            {
                // arrange
                var sut = Maybe<int>.Some(Fixture.Create<int>());

                // act
                // assert
                sut.Should().BeSomeVariant(because: "{0} is created using domestic static Some() method and should be treated as 'some'", sut);
            }

            [Fact]
            public void GenericSome_ShouldBeSome()
            {
                // arrange
                var sut = Maybe.Some(Fixture.Create<int>());

                // act
                // assert
                sut.Should().BeSomeVariant(because: "{0} is created using generic Some<T>() method and should be treated as 'some'", sut);
            }
        }
    }
}
