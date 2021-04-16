using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Monads.Extensions;
using System;

namespace Monads.FluentAssertions
{
    public class MaybeAssertions<T>
    {
        public MaybeAssertions(Maybe<T>? subject)
        {
            Subject = subject;
        }

        public Maybe<T>? Subject { get; }

        public AndConstraint<MaybeAssertions<T>> Be(Maybe<T> expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Maybe<T>} to be {0}{reason}, but found {1}.", expected, Subject);
            return new AndConstraint<MaybeAssertions<T>>(this);
        }
        public AndConstraint<MaybeAssertions<T>> NotBe(Maybe<T> expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.Equals(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Maybe<T>} to be {0}{reason}, but found {1}.", expected, Subject);
            return new AndConstraint<MaybeAssertions<T>>(this);
        }

        public AndConstraint<MaybeAssertions<T>> BeNoneVariant(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != null && Subject.Value.Match(some: _ => false, none: () => true))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Maybe<T>} to be 'none'{reason}, but found {1}.", Subject);
            return new AndConstraint<MaybeAssertions<T>>(this);
        }
        public AndConstraint<MaybeAssertions<T>> BeSomeVariant(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != null && Subject.Value.Match(some: _ => true, none: () => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:Maybe<T>} to be 'some'{reason}, but found {1}.", Subject);
            return new AndConstraint<MaybeAssertions<T>>(this);
        }

        public AndConstraint<ObjectAssertions> BeSomeOf(T value, string because = "", params object[] becauseArgs) =>
            BeSomeVariant(because, becauseArgs)
                .And.Subject.Value.Match(
                    some: e => e.Should().Be(value, because, becauseArgs),
                    none: () => throw new InvalidOperationException());

        public void BeSomeOfEquivalent<TExpectation>(TExpectation value, string because = "", params object[] becauseArgs) =>
            BeSomeVariant(because, becauseArgs)
                .And.Subject.Value.Act(
                    some: e => e.Should().BeEquivalentTo(value, because, becauseArgs),
                    none: () => throw new InvalidOperationException());

        public void BeSomeOfEquivalent<TExpectation>(
            TExpectation value,
            Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
            string because = "",
            params object[] becauseArgs) =>
            BeSomeVariant(because, becauseArgs)
                .And.Subject.Value.Act(
                    some: e => e.Should().BeEquivalentTo(value, config, because, becauseArgs),
                    none: () => throw new InvalidOperationException());
    }
}