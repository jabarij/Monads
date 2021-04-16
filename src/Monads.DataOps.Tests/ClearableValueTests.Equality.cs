using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.DataOps.Tests
{
    partial class ClearableValueTests
    {
        public class Equality : ClearableValueTests
        {
            public Equality(TestFixture testFixture) : base(testFixture) { }

            [Fact]
            public void NoActionAndDefaultClearableValue_ShouldBeEqual()
            {
                // arrange
                var noAction = ClearableValue<int>.NoAction();
                var @default = default(ClearableValue<int>);

                // act
                // assert
                noAction.Equals(@default).Should().BeTrue();
                @default.Equals(noAction).Should().BeTrue();
            }

            [Fact]
            public void NoActionAndSetOfDefaultValue_ShouldNotBeEqual()
            {
                // arrange
                var noAction = ClearableValue<int>.NoAction();
                var set = ClearableValue<int>.Set(default(int));

                // act
                // assert
                noAction.Equals(set).Should().BeFalse();
                set.Equals(noAction).Should().BeFalse();
            }

            [Fact]
            public void NoActionAndClear_ShouldNotBeEqual()
            {
                // arrange
                var noAction = ClearableValue<int>.NoAction();
                var clear = ClearableValue<int>.Clear();

                // act
                // assert
                noAction.Equals(clear).Should().BeFalse();
                clear.Equals(noAction).Should().BeFalse();
            }

            [Fact]
            public void ClearAndSetOfDefaultValue_ShouldNotBeEqual()
            {
                // arrange
                var clear = ClearableValue<int>.Clear();
                var set = ClearableValue<int>.Set(default(int));

                // act
                // assert
                clear.Equals(set).Should().BeFalse();
                set.Equals(clear).Should().BeFalse();
            }

            [Fact]
            public void SetOfSameValues_ShouldBeEqual()
            {
                // arrange
                var set1 = ClearableValue<int>.Set(1);
                var set2 = ClearableValue<int>.Set(1);

                // act
                // assert
                set1.Equals(set2).Should().BeTrue();
                set2.Equals(set1).Should().BeTrue();
            }

            [Fact]
            public void SetOfDifferentValues_ShouldNotBeEqual()
            {
                // arrange
                var set1 = ClearableValue<int>.Set(1);
                var set2 = ClearableValue<int>.Set(2);

                // act
                // assert
                set1.Equals(set2).Should().BeFalse();
                set2.Equals(set1).Should().BeFalse();
            }
        }
    }
}
