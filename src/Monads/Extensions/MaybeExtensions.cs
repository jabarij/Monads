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
            ? Maybe<T>.Some(value.Value)
            : Maybe<T>.None();

        public static Maybe<T> AsMaybe<T>(this T value) where T : class =>
            value is null
            ? Maybe<T>.None()
            : Maybe<T>.Some(value);

        public static void Act<T>(this Maybe<T> maybe, Action<T> some, Action none) =>
            maybe.Match(
                some: e => some.ToVoidFunc().Invoke(e),
                none: () => none.ToVoidFunc().Invoke());

        public static T Reduce<T>(this Maybe<T> maybe, T defaultValue = default(T)) =>
            maybe.Match(
                some: Functions.Id,
                none: () => defaultValue);

        public static T ReduceWith<T>(this Maybe<T> maybe, Func<T> defaultValue) =>
            maybe.Match(
                some: Functions.Id,
                none: defaultValue);

        public static T? ReduceOrNull<T>(this Maybe<T> maybe) where T : struct =>
            maybe.Match(
                some: Functions.NullableId,
                none: () => (T?)null);

        public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) =>
            maybe.Match(
                some: Functions.Id,
                none: Maybe<T>.None);

        public static Type GetUnderlyingType<T>(this Maybe<T> _) =>
            typeof(T);
    }
}
