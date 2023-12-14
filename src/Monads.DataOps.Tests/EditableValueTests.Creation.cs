using AutoFixture;
using Monads.DataOps.FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.DataOps.Tests;

partial class EditableValueTests
{
    public class Creation : EditableValueTests
    {
        public Creation(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void DefaultValue_ShouldBeNoAction()
        {
            // arrange
            var sut = default(EditableValue<int>);

            // act=
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is of default value and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void ParameterlessConstructor_ShouldBeNoAction()
        {
            // arrange
            var sut = new EditableValue<int>();

            // act
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is created using parameterless constructor and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void ConstructorWithValue_ShouldBeUpdate()
        {
            // arrange
            var sut = new EditableValue<int>(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeUpdateVariant(because: "{0} is created using constructor with value parameter and should be treated as 'update'", sut);
        }

        [Fact]
        public void DomesticNoAction_ShouldBeNoAction()
        {
            // arrange
            var sut = EditableValue<int>.NoAction();

            // act
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is created using domestic static NoAction() method and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void GenericNoAction_ShouldBeNoAction()
        {
            // arrange
            var sut = EditableValue.NoAction(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeNoActionVariant(because: "{0} is created using generic NoAction<T>() method and should be treated as 'noAction'", sut);
        }

        [Fact]
        public void DomesticUpdate_ShouldBeUpdate()
        {
            // arrange
            var sut = EditableValue<int>.Update(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeUpdateVariant(because: "{0} is created using domestic static Update() method and should be treated as 'update'", sut);
        }

        [Fact]
        public void GenericUpdate_ShouldBeUpdate()
        {
            // arrange
            var sut = EditableValue.Update(Fixture.Create<int>());

            // act
            // assert
            sut.Should().BeUpdateVariant(because: "{0} is created using generic Update<T>() method and should be treated as 'update'", sut);
        }
    }
}