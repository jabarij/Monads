using AutoFixture;
using Monads.DataOps.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.DataOps.Tests;

partial class ClearableValueTests
{
    public class Creation : ClearableValueTests
    {
        public Creation(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void DefaultValue_ShouldBeNoAction()
        {
            // arrange
            var sut = default(ClearableValue<int>);

            // act=
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is of default value and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void ParameterlessConstructor_ShouldBeNoAction()
        {
            // arrange
            var sut = new ClearableValue<int>();

            // act
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is created using parameterless constructor and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void ConstructorWithValue_ShouldBeSet()
        {
            // arrange
            var sut = new ClearableValue<int>(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeSetVariant(because: "{0} is created using constructor with value parameter and should be treated as 'set'", sut);
        }

        [Fact]
        public void DomesticNoAction_ShouldBeNoAction()
        {
            // arrange
            var sut = ClearableValue<int>.NoAction();

            // act
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is created using domestic static NoAction() method and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void GenericNoAction_ShouldBeNoAction()
        {
            // arrange
            var sut = ClearableValue.NoAction(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is created using generic NoAction<T>() method and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void DomesticSet_ShouldBeSet()
        {
            // arrange
            var sut = ClearableValue<int>.Set(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeSetVariant(because: "{0} is created using domestic static Set() method and should be treated as 'set'", sut);
        }

        [Fact]
        public void GenericSet_ShouldBeSet()
        {
            // arrange
            var sut = ClearableValue.Set(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeSetVariant(because: "{0} is created using generic Set<T>() method and should be treated as 'set'", sut);
        }
    }
}