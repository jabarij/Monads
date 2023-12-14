using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class MaybeTests
{
    public class Equality : MaybeTests
    {
        public Equality(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void NoneAndDefaultMaybe_ShouldBeEqual()
        {
            // arrange
            var none = Maybe<int>.None();
            var @default = default(Maybe<int>);

            // act
            // assert
            none.Equals(@default).Should().BeTrue();
            @default.Equals(none).Should().BeTrue();
        }

        [Fact]
        public void NoneAndSomeOfDefaultValue_ShouldNotBeEqual()
        {
            // arrange
            var none = Maybe<int>.None();
            var some = Maybe<int>.Some(default);

            // act
            // assert
            none.Equals(some).Should().BeFalse();
            some.Equals(none).Should().BeFalse();
        }

        [Fact]
        public void SomeOfSameValues_ShouldBeEqual()
        {
            // arrange
            var some1 = Maybe<int>.Some(1);
            var some2 = Maybe<int>.Some(1);

            // act
            // assert
            some1.Equals(some2).Should().BeTrue();
            some2.Equals(some1).Should().BeTrue();
        }

        [Fact]
        public void SomeOfDifferentValues_ShouldNotBeEqual()
        {
            // arrange
            var some1 = Maybe<int>.Some(1);
            var some2 = Maybe<int>.Some(2);

            // act
            // assert
            some1.Equals(some2).Should().BeFalse();
            some2.Equals(some1).Should().BeFalse();
        }
    }
}