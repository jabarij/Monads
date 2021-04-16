using DotNetExtensions;
using System;

namespace Monads.DataOps.Extensions
{
    public static class ClearableValueExtensions
    {
        public static void Act<T>(this ClearableValue<T> value, Action<T> set, Action clear, Action noAction = null) =>
            value.Match(
                set: set.ToVoidFunc(),
                clear: clear.ToVoidFunc(),
                noAction: noAction.ToVoidFuncOrEmpty());

        public static bool IsSet<T>(this ClearableValue<T> value) =>
            value.Match(
                set: _ => true,
                clear: () => false,
                noAction: () => false);

        public static bool IsClear<T>(this ClearableValue<T> value) =>
            value.Match(
                set: _ => false,
                clear: () => true,
                noAction: () => false);

        public static bool IsNoAction<T>(this ClearableValue<T> value) =>
            value.Match(
                set: _ => true,
                clear: () => false,
                noAction: () => false);

        public static ClearableValue<T> ClearIfNull<T>(this T value) where T : class =>
            value is null
            ? ClearableValue<T>.Clear()
            : ClearableValue.Set(value);

        public static ClearableValue<T> ClearIfNull<T>(this T? value) where T : struct =>
            value.HasValue
            ? ClearableValue.Set(value.Value)
            : ClearableValue<T>.Clear();

        public static ClearableValue<T> SetIfNotNull<T>(this T? value) where T : struct =>
            value.HasValue
            ? ClearableValue.Set(value.Value)
            : ClearableValue<T>.NoAction();

        public static ClearableValue<string> ClearIfNullOrEmpty(this string value) =>
            !string.IsNullOrEmpty(value)
            ? ClearableValue.Set(value)
            : ClearableValue<string>.Clear();

        public static ClearableValue<string> ClearIfNullOrWhiteSpace(this string value) =>
            !string.IsNullOrWhiteSpace(value)
            ? ClearableValue.Set(value)
            : ClearableValue<string>.Clear();

        public static T ReduceWith<T>(this ClearableValue<T> value, Func<T> clear, Func<T> noAction) =>
            value.Match(
                set: e => e,
                clear: clear,
                noAction: noAction);

        public static T Reduce<T>(this ClearableValue<T> value, T clear = default(T), T noAction = default(T)) =>
            value.Match(
                set: e => e,
                clear: () => clear,
                noAction: () => noAction);

        public static T? ReduceOrNull<T>(this ClearableValue<T> value, T? clear = null, T? noAction = null) where T : struct =>
            value
            .AsNullable()
            .Reduce(
                clear: clear,
                noAction: noAction);

        public static ClearableValue<T?> AsNullable<T>(this ClearableValue<T> value) where T : struct =>
            value.Map(e => (T?)e);
    }
}
