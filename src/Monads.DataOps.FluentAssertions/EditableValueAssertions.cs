using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Monads.DataOps.Extensions;

namespace Monads.DataOps.FluentAssertions;

public class EditableValueAssertions<T>
{
    public EditableValueAssertions(EditableValue<T>? subject)
    {
        Subject = subject;
    }

    public EditableValue<T>? Subject { get; }

    public AndConstraint<EditableValueAssertions<T>> Be(EditableValue<T> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:EditableValue<T>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<EditableValueAssertions<T>>(this);
    }
    public AndConstraint<EditableValueAssertions<T>> NotBe(EditableValue<T> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:EditableValue<T>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<EditableValueAssertions<T>>(this);
    }

    public AndConstraint<EditableValueAssertions<T>> BeNoActionVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(update: _ => false, noAction: () => true))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:EditableValue<T>} to be 'noAction'{reason}, but found {1}.", Subject);
        return new AndConstraint<EditableValueAssertions<T>>(this);
    }
    public AndConstraint<EditableValueAssertions<T>> BeUpdateVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(update: _ => true, noAction: () => false))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:EditableValue<T>} to be 'update'{reason}, but found {1}.", Subject);
        return new AndConstraint<EditableValueAssertions<T>>(this);
    }

    public AndConstraint<ObjectAssertions> BeUpdateOf(T value, string because = "", params object[] becauseArgs) =>
        BeUpdateVariant(because, becauseArgs)
            .And.Subject.Value.Match(
                update: e => e.Should().Be(value, because, becauseArgs),
                noAction: () => throw new InvalidOperationException());

    public void BeUpdateOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
        BeUpdateVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                update: e => e.Should().BeEquivalentTo(value, because, becauseArgs),
                noAction: () => throw new InvalidOperationException());

    public void BeUpdateOfEquivalent<TExpectation>(
        TExpectation value,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs) =>
        BeUpdateVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                update: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs),
                noAction: () => throw new InvalidOperationException());
}