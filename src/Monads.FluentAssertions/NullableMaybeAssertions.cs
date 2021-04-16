using FluentAssertions;
using FluentAssertions.Execution;

namespace Monads.FluentAssertions
{
    public class NullableMaybeAssertions<T> : MaybeAssertions<T>
    {
        public NullableMaybeAssertions(Maybe<T>? subject) : base(subject) { }

        public AndConstraint<NullableMaybeAssertions<T>> NotBeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect null{reason}.");
            return new AndConstraint<NullableMaybeAssertions<T>>(this);
        }
        public AndConstraint<NullableMaybeAssertions<T>> NotBeNullOrNone(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.Match(some: _ => true, none: () => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect neither null nor 'none' value{reason}.");
            return new AndConstraint<NullableMaybeAssertions<T>>(this);
        }

        public AndConstraint<NullableMaybeAssertions<T>> BeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableMaybeAssertions<T>>(this);
        }
        public AndConstraint<NullableMaybeAssertions<T>> BeNullOrNone(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || Subject.Value.Match(some: _ => false, none: () => true))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null or 'none' value{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableMaybeAssertions<T>>(this);
        }
        public AndConstraint<NullableMaybeAssertions<T>> Be(Maybe<T>? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Address} to be {0}{reason}, but found {1}.", expected, Subject);
            return new AndConstraint<NullableMaybeAssertions<T>>(this);
        }
    }
}