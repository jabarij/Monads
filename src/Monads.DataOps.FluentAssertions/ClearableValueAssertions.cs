using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Monads.DataOps.Extensions;

namespace Monads.DataOps.FluentAssertions;

public class ClearableValueAssertions<T>
{
    public ClearableValueAssertions(ClearableValue<T>? subject)
    {
        Subject = subject;
    }

    public ClearableValue<T>? Subject { get; }

    public AndConstraint<ClearableValueAssertions<T>> Be(ClearableValue<T> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:ClearableValue<T>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<ClearableValueAssertions<T>>(this);
    }
    public AndConstraint<ClearableValueAssertions<T>> NotBe(ClearableValue<T> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:ClearableValue<T>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<ClearableValueAssertions<T>>(this);
    }

    public AndConstraint<ClearableValueAssertions<T>> BeNoActionVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(set: _ => false, clear: () => false, noAction: () => true))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:ClearableValue<T>} to be 'noAction'{reason}, but found {1}.", Subject);
        return new AndConstraint<ClearableValueAssertions<T>>(this);
    }
    public AndConstraint<ClearableValueAssertions<T>> BeSetVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(set: _ => true, clear: () => false, noAction: () => false))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:ClearableValue<T>} to be 'set'{reason}, but found {1}.", Subject);
        return new AndConstraint<ClearableValueAssertions<T>>(this);
    }
    public AndConstraint<ClearableValueAssertions<T>> BeClearVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(set: _ => false, clear: () => true, noAction: () => false))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:ClearableValue<T>} to be 'clear'{reason}, but found {1}.", Subject);
        return new AndConstraint<ClearableValueAssertions<T>>(this);
    }

    public AndConstraint<ObjectAssertions> BeSetOf(T value, string because = "", params object[] becauseArgs) =>
        BeSetVariant(because, becauseArgs)
            .And.Subject.Value.Match(
                set: e => e.Should().Be(value, because, becauseArgs),
                clear: () => throw new InvalidOperationException(),
                noAction: () => throw new InvalidOperationException());

    public void BeSetOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
        BeSetVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                set: e => e.Should().BeEquivalentTo(value, because, becauseArgs),
                clear: () => throw new InvalidOperationException(),
                noAction: () => throw new InvalidOperationException());

    public void BeSetOfEquivalent<TExpectation>(
        TExpectation value,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs) =>
        BeSetVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                set: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs),
                clear: () => throw new InvalidOperationException(),
                noAction: () => throw new InvalidOperationException());
}