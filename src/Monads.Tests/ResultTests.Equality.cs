using FluentAssertions;
using Monads.TestAbstractions;
using Xunit;

namespace Monads.Tests;

partial class ResultTests
{
    public class Equality : ResultTests
    {
        public Equality(TestFixture testFixture) : base(testFixture) { }

        [Fact]
        public void ErrorWithDefaultValueAndDefaultResult_ShouldBeEqual()
        {
            // arrange
            var error = Result<int, string>.Error(default);
            var @default = default(Result<int, string>);

            // act
            // assert
            error.Equals(@default).Should().BeTrue();
            @default.Equals(error).Should().BeTrue();
        }

        [Fact]
        public void OkOfSameValues_ShouldBeEqual()
        {
            // arrange
            var ok1 = Result<int, int>.Ok(1);
            var ok2 = Result<int, int>.Ok(1);

            // act
            // assert
            ok1.Equals(ok2).Should().BeTrue();
            ok2.Equals(ok1).Should().BeTrue();
        }

        [Fact]
        public void OkOfDifferentValues_ShouldNotBeEqual()
        {
            // arrange
            var ok1 = Result<int, int>.Ok(1);
            var ok2 = Result<int, int>.Ok(2);

            // act
            // assert
            ok1.Equals(ok2).Should().BeFalse();
            ok2.Equals(ok1).Should().BeFalse();
        }

        [Fact]
        public void ErrorOfSameValues_ShouldBeEqual()
        {
            // arrange
            var error1 = Result<int, int>.Error(1);
            var error2 = Result<int, int>.Error(1);

            // act
            // assert
            error1.Equals(error2).Should().BeTrue();
            error2.Equals(error1).Should().BeTrue();
        }

        [Fact]
        public void ErrorOfDifferentValues_ShouldNotBeEqual()
        {
            // arrange
            var error1 = Result<int, int>.Error(1);
            var error2 = Result<int, int>.Error(2);

            // act
            // assert
            error1.Equals(error2).Should().BeFalse();
            error2.Equals(error1).Should().BeFalse();
        }

        [Fact]

        public void OkAndErrorOfSameValues_ShouldNotBeEqual()
        {
            // arrange
            var ok = Result<int, int>.Ok(1);
            var error = Result<int, int>.Error(1);

            // act
            // assert
            ok.Equals(error).Should().BeFalse();
            error.Equals(ok).Should().BeFalse();
        }
    }
}