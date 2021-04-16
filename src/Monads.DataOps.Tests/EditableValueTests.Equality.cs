using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.DataOps.Tests
{
    partial class EditableValueTests
    {
        public class Equality : EditableValueTests
        {
            public Equality(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void NoActionAndDefaultEditableValue_ShouldBeEqual()
            {
                // arrange
                var noAction = EditableValue<int>.NoAction();
                var @default = default(EditableValue<int>);

                // act
                // assert
                noAction.Equals(@default).Should().BeTrue();
                @default.Equals(noAction).Should().BeTrue();
            }

            [Fact]
            public void NoActionAndUpdateOfDefaultValue_ShouldNotBeEqual()
            {
                // arrange
                var noAction = EditableValue<int>.NoAction();
                var update = EditableValue<int>.Update(default(int));

                // act
                // assert
                noAction.Equals(update).Should().BeFalse();
                update.Equals(noAction).Should().BeFalse();
            }

            [Fact]
            public void UpdateOfSameValues_ShouldBeEqual()
            {
                // arrange
                var update1 = EditableValue<int>.Update(1);
                var update2 = EditableValue<int>.Update(1);

                // act
                // assert
                update1.Equals(update2).Should().BeTrue();
                update2.Equals(update1).Should().BeTrue();
            }

            [Fact]
            public void UpdateOfDifferentValues_ShouldNotBeEqual()
            {
                // arrange
                var update1 = EditableValue<int>.Update(1);
                var update2 = EditableValue<int>.Update(2);

                // act
                // assert
                update1.Equals(update2).Should().BeFalse();
                update2.Equals(update1).Should().BeFalse();
            }
        }
    }
}
