using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Monads.Extensions;

namespace Monads.FluentAssertions;

public class EitherAssertions<TLeft, TRight>
{
    public EitherAssertions(Either<TLeft, TRight>? subject)
    {
        Subject = subject;
    }

    public Either<TLeft, TRight>? Subject { get; }

    public AndConstraint<EitherAssertions<TLeft, TRight>> Be(Either<TLeft, TRight> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Either<TLeft, TRight>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<EitherAssertions<TLeft, TRight>>(this);
    }
    public AndConstraint<EitherAssertions<TLeft, TRight>> NotBe(Either<TLeft, TRight> expected, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(!Subject.Equals(expected))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Either<TLeft, TRight>} to be {0}{reason}, but found {1}.", expected, Subject);
        return new AndConstraint<EitherAssertions<TLeft, TRight>>(this);
    }

    public AndConstraint<EitherAssertions<TLeft, TRight>> BeLeftVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(left: _ => true, right: _ => false))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Either<TLeft, TRight>} to be 'left'{reason}, but found {1}.", Subject);
        return new AndConstraint<EitherAssertions<TLeft, TRight>>(this);
    }
    public AndConstraint<EitherAssertions<TLeft, TRight>> BeRightVariant(string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .ForCondition(Subject != null && Subject.Value.Match(left: _ => false, right: _ => true))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Either<TLeft, TRight>} to be 'right'{reason}, but found {1}.", Subject);
        return new AndConstraint<EitherAssertions<TLeft, TRight>>(this);
    }

    public AndConstraint<ObjectAssertions> BeLeftOf(TLeft value, string because = "", params object[] becauseArgs) =>
        BeLeftVariant(because, becauseArgs)
            .And.Subject.Value.Match(
                left: e => e.Should().Be(value, because, becauseArgs),
                right: _ => throw new InvalidOperationException());
    public AndConstraint<ObjectAssertions> BeRightOf(TRight value, string because = "", params object[] becauseArgs) =>
        BeRightVariant(because, becauseArgs)
            .And.Subject.Value.Match(
                left: _ => throw new InvalidOperationException(),
                right: e => e.Should().Be(value, because, becauseArgs));

    public void BeLeftOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
        BeLeftVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                left: e => e.Should().BeEquivalentTo(value, because, becauseArgs),
                right: _ => throw new InvalidOperationException());
    public void BeRightOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
        BeRightVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                left: _ => throw new InvalidOperationException(),
                right: e => e.Should().BeEquivalentTo(value, because, becauseArgs));

    public void BeLeftOfEquivalent<TExpectation>(
        TExpectation value,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs) =>
        BeLeftVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                left: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs),
                right: _ => throw new InvalidOperationException());
    public void BeRightOfEquivalent<TExpectation>(
        TExpectation value,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs) =>
        BeRightVariant(because, becauseArgs)
            .And.Subject.Value.Act(
                left: _ => throw new InvalidOperationException(),
                right: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs));
}