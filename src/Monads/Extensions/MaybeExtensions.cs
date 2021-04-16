using DotNetExtensions;
using System;

namespace Monads.Extensions
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class MaybeExtensions
    {
        public static bool IsSome<T>(this Maybe<T> maybe) =>
            maybe.Match(
                some: _ => true,
                none: () => false);

        public static bool IsNone<T>(this Maybe<T> maybe) =>
            maybe.Match(
                some: _ => false,
                none: () => true);

        public static Maybe<T> AsMaybe<T>(this T? value) where T : struct =>
            value.HasValue
            ? Maybe.Some(value.Value)
            : Maybe.None<T>();

        public static Maybe<T> AsMaybe<T>(this T value) where T : class =>
            value is null
            ? Maybe.None<T>()
            : Maybe.Some(value);

        public static TResult Match<T, TResult>(this Maybe<T> maybe, Func<T, TResult> some, TResult defaultValue = default(TResult)) =>
            maybe.Match(
                some: some,
                none: () => defaultValue);

        public static void Act<T>(this Maybe<T> maybe, Action<T> some, Action none) =>
            maybe.Match(
                some: e => some.ToVoidFunc().Invoke(e),
                none: () => none.ToVoidFunc().Invoke());

        public static T Reduce<T>(this Maybe<T> maybe, T defaultValue = default(T)) =>
            maybe.Match(e => e, () => defaultValue);

        public static T ReduceWith<T>(this Maybe<T> maybe, Func<T> defaultValue) =>
            maybe.Match(e => e, defaultValue);

        public static T? ReduceOrNull<T>(this Maybe<T> maybe) where T : struct =>
            maybe.Match(e => e, () => (T?)null);
    }
}
