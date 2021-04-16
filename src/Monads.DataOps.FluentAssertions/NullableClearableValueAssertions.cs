using FluentAssertions;
using FluentAssertions.Execution;

namespace Monads.DataOps.FluentAssertions
{
    public class NullableClearableValueAssertions<T> : ClearableValueAssertions<T>
    {
        public NullableClearableValueAssertions(ClearableValue<T>? subject) : base(subject) { }

        public AndConstraint<NullableClearableValueAssertions<T>> NotBeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect null{reason}.");
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }
        public AndConstraint<NullableClearableValueAssertions<T>> NotBeNullOrNoActionVariant(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.Match(set: _ => true, clear: () => true, noAction: () => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect neither null nor 'noAction' value{reason}.");
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }
        public AndConstraint<NullableClearableValueAssertions<T>> NotBeNullOrClearVariant(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && Subject.Value.Match(set: _ => true, clear: () => false, noAction: () => true))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect neither null nor 'clear' value{reason}.");
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }

        public AndConstraint<NullableClearableValueAssertions<T>> BeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }
        public AndConstraint<NullableClearableValueAssertions<T>> BeNullOrNoActionVariant(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || Subject.Value.Match(set: _ => false, clear: () => false, noAction: () => true))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null or 'noAction' value{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }
        public AndConstraint<NullableClearableValueAssertions<T>> BeNullOrClearVariant(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue || Subject.Value.Match(set: _ => false, clear: () => true, noAction: () => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected null or 'clear' value{reason}, but found {0}.", Subject);
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }
        public AndConstraint<NullableClearableValueAssertions<T>> Be(ClearableValue<T>? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Address} to be {0}{reason}, but found {1}.", expected, Subject);
            return new AndConstraint<NullableClearableValueAssertions<T>>(this);
        }
    }
}