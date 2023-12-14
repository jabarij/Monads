using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Monads.Extensions;

namespace Monads.FluentAssertions;

public class ResultAssertions<TOk, TError>
{
    public ResultAssertions(Result<TOk, TError>? subject)
    {
        Subject = subject;
    }

    public Result<TOk, TError>? Subject { get; }

    public AndConstraint<ResultAssertions<TOk, TError>> Be(Result<TOk, TError> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result<TOk, TError>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<ResultAssertions<TOk, TError>>(this);
    }
    public AndConstraint<ResultAssertions<TOk, TError>> NotBe(Result<TOk, TError> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result<TOk, TError>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<ResultAssertions<TOk, TError>>(this);
    }

    public AndConstraint<ResultAssertions<TOk, TError>> BeOkVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(ok: _ => true, error: _ => false))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result<TOk, TError>} to be 'ok'{reason}, but found {1}.", Subject);
        return new AndConstraint<ResultAssertions<TOk, TError>>(this);
    }
    public AndConstraint<ResultAssertions<TOk, TError>> BeErrorVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(ok: _ => false, error: _ => true))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result<TOk, TError>} to be 'error'{reason}, but found {1}.", Subject);
        return new AndConstraint<ResultAssertions<TOk, TError>>(this);
    }

    public AndConstraint<ObjectAssertions> BeOkOf(TOk value, string because = "", params object[] becauseArgs) =>
        BeOkVariant(because, becauseArgs)
            .And.Subject.Value.Match(
                ok: e => e.Should().Be(value, because, becauseArgs),
                error: _ => throw new InvalidOperationException());
    public AndConstraint<ObjectAssertions> BeErrorOf(TError value, string because = "", params object[] becauseArgs) =>
        BeErrorVariant(because, becauseArgs)
            .And.Subject.Value.Match(
                ok: _ => throw new InvalidOperationException(),
                error: e => e.Should().Be(value, because, becauseArgs));

    public void BeOkOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
        BeOkVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                ok: e => e.Should().BeEquivalentTo(value, because, becauseArgs),
                error: _ => throw new InvalidOperationException());
    public void BeErrorOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
        BeErrorVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                ok: _ => throw new InvalidOperationException(),
                error: e => e.Should().BeEquivalentTo(value, because, becauseArgs));

    public void BeOkOfEquivalent<TExpectation>(
        TExpectation value,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs) =>
        BeOkVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                ok: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs),
                error: _ => throw new InvalidOperationException());
    public void BeErrorOfEquivalent<TExpectation>(
        TExpectation value,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs) =>
        BeErrorVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                ok: _ => throw new InvalidOperationException(),
                error: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs));
}