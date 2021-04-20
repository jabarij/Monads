using FluentAssertions;
using FluentAssertions.Execution;

namespace Monads.FluentAssertions
{
    public class NullableResultAssertions<TOk, TError> : ResultAssertions<TOk, TError>
    {
        public NullableResultAssertions(Result<TOk, TError>? subject) : base(subject) { }

        public AndConstraint<NullableResultAssertions<TOk, TError>> NotBeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect null{reason}.");
            return new AndConstraint<NullableResultAssertions<TOk, TError>>(this);
        }

        public AndConstraint<NullableResultAssertions<TOk, TError>> BeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableResultAssertions<TOk, TError>>(this);
        }
        public AndConstraint<NullableResultAssertions<TOk, TError>> Be(Result<TOk, TError>? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Address} to be {0}{reason}, but found {1}.", expected, Subject);
            return new AndConstraint<NullableResultAssertions<TOk, TError>>(this);
        }
    }
}