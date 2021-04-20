using DotNetExtensions;
using System;

namespace Monads.DataOps.Extensions
{
    public static class EditableValueExtensions
    {
        public static void Act<T>(this EditableValue<T> value, Action<T> update, Action noAction = null) =>
            value.Match(
                update: update.ToVoidFunc(),
                noAction: noAction.ToVoidFuncOrEmpty());

        public static bool IsUpdate<T>(this EditableValue<T> value) =>
            value.Match(
                update: _ => true,
                noAction: () => false);

        public static bool IsNoAction<T>(this EditableValue<T> value) =>
            value.Match(
                update: _ => false,
                noAction: () => true);

        public static EditableValue<T> UpdateIfNotNull<T>(this T? value) where T : struct =>
            value != null
            ? EditableValue<T>.Update(value.Value)
            : EditableValue<T>.NoAction();

        public static EditableValue<T> UpdateIfNotNull<T>(this T value) where T : class =>
            value != null
            ? EditableValue<T>.Update(value)
            : EditableValue<T>.NoAction();

        public static EditableValue<string> UpdateIfNotNullOrEmpty(this string value) =>
            !string.IsNullOrEmpty(value)
            ? EditableValue<string>.Update(value)
            : EditableValue<string>.NoAction();

        public static EditableValue<string> UpdateIfNotNullOrWhiteSpace(this string value) =>
            !string.IsNullOrWhiteSpace(value)
            ? EditableValue<string>.Update(value)
            : EditableValue<string>.NoAction();

        public static EditableValue<T> NoActionIfNull<T>(this EditableValue<T>? value) =>
            value
            ?? EditableValue<T>.NoAction();

        public static EditableValue<T?> AsNullable<T>(this EditableValue<T> value) where T : struct =>
            value.Map(Functions.NullableId);

        public static ClearableValue<T> AsClearable<T>(this EditableValue<T> value) =>
            value.Match(
                update: ClearableValue<T>.Set,
                noAction: ClearableValue<T>.NoAction);

        public static T ReduceWith<T>(this EditableValue<T> value, Func<T> noAction) =>
            value.Match(
                update: Functions.Id,
                noAction: noAction);

        public static T Reduce<T>(this EditableValue<T> value, T defaultValue = default(T)) =>
            value.Match(
                update: Functions.Id,
                noAction: () => defaultValue);

        public static T? ReduceOrNull<T>(this EditableValue<T> value, T? defaultValue) where T : struct =>
            value.Match(
                update: Functions.NullableId,
                noAction: () => defaultValue);

        public static T ReduceOrDefault<T>(this EditableValue<T> value) =>
            value.Match(
                update: Functions.Id,
                noAction: () => default(T));

        public static TResult ReduceMap<T, TResult>(this EditableValue<T> value, Func<T, TResult> map, Func<TResult> noAction) =>
            value.Match(
                update: map,
                noAction: noAction);

        public static TResult ReduceMap<T, TResult>(this EditableValue<T> value, Func<T, TResult> map, TResult noActionValue = default(TResult)) =>
            value.Match(
                update: map,
                noAction: () => noActionValue);
    }
}
