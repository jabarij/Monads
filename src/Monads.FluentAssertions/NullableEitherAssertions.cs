using FluentAssertions;
using FluentAssertions.Execution;

namespace Monads.FluentAssertions
{
    public class NullableEitherAssertions<TLeft, TRight> : EitherAssertions<TLeft, TRight>
    {
        public NullableEitherAssertions(Either<TLeft, TRight>? subject) : base(subject) { }

        public AndConstraint<NullableEitherAssertions<TLeft, TRight>> NotBeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect null{reason}.");
            return new AndConstraint<NullableEitherAssertions<TLeft, TRight>>(this);
        }

        public AndConstraint<NullableEitherAssertions<TLeft, TRight>> BeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableEitherAssertions<TLeft, TRight>>(this);
        }
        public AndConstraint<NullableEitherAssertions<TLeft, TRight>> Be(Either<TLeft, TRight>? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Address} to be {0}{reason}, but found {1}.", expected, Subject);
            return new AndConstraint<NullableEitherAssertions<TLeft, TRight>>(this);
        }
    }
}