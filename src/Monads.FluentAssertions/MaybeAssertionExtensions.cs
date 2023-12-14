namespace Monads.FluentAssertions;

public static class MaybeAssertionExtensions
{
    public static MaybeAssertions<T> Should<T>(this Maybe<T> actualValue) => new(actualValue);
    public static NullableMaybeAssertions<T> Should<T>(this Maybe<T>? actualValue) => new(actualValue);
}