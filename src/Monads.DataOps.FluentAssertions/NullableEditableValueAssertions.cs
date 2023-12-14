using FluentAssertions;
using FluentAssertions.Execution;

namespace Monads.DataOps.FluentAssertions;

public class NullableEditableValueAssertions<T> : EditableValueAssertions<T>
{
    public NullableEditableValueAssertions(EditableValue<T>? subject) : base(subject) { }

    public AndConstraint<NullableEditableValueAssertions<T>> NotBeNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect null{reason}.");
        return new AndConstraint<NullableEditableValueAssertions<T>>(this);
    }
    public AndConstraint<NullableEditableValueAssertions<T>> NotBeNullOrNoAction(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.HasValue && Subject.Value.Match(update: _ => true, noAction: () => false))
            .BecauseOf(because, becauseArgs)
            .FailWith("Did not expect neither null nor 'noAction' value{reason}.");
        return new AndConstraint<NullableEditableValueAssertions<T>>(this);
    }

    public AndConstraint<NullableEditableValueAssertions<T>> BeNull(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.HasValue)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected null{reason}, but found {0}.", Subject);
        return new AndConstraint<NullableEditableValueAssertions<T>>(this);
    }
    public AndConstraint<NullableEditableValueAssertions<T>> BeNullOrNoAction(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.HasValue || Subject.Value.Match(update: _ => false, noAction: () => true))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected null or 'noAction' value{reason}, but found {0}.", Subject);
        return new AndConstraint<NullableEditableValueAssertions<T>>(this);
    }
    public AndConstraint<NullableEditableValueAssertions<T>> Be(EditableValue<T>? expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject == expected)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Address} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<NullableEditableValueAssertions<T>>(this);
    }
}