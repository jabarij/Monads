using DotNetExtensions;
using System;

namespace Monads.DataOps
{
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
    public struct EditableValue<T> : IEquatable<EditableValue<T>>, IEquatable<T>
    {
        private string DebuggerDisplay => Match(
            update: e => $"update({e})",
            noAction: () => $"noAction<{typeof(T).Name}>");

        private readonly T _value;
        private readonly bool _update;

        public EditableValue(T value)
        {
            _value = value;
            _update = true;
        }

        public TResult Match<TResult>(Func<T, TResult> update, Func<TResult> noAction) =>
            _update
            ? update(_value)
            : noAction();

        public EditableValue<TResult> Map<TResult>(Func<T, TResult> map) =>
            _update
            ? new EditableValue<TResult>(map(_value))
            : new EditableValue<TResult>();

        public static EditableValue<T> NoAction() =>
            new EditableValue<T>();
        public static EditableValue<T> Update(T value) =>
            new EditableValue<T>(value);

        #region Equality

        public override string ToString() =>
            Match(
                update: e => string.Concat("update(", e, ")"),
                noAction: () => string.Concat("noAction<", typeof(T).Name, ">"));

        public bool Equals(T value) =>
            Equals(_value, value);
        public bool Equals(EditableValue<T> other) =>
            _update == other._update
            && (_update
                ? Equals(_value, other._value)
                : true);
        public override bool Equals(object obj) =>
            obj is EditableValue<T> other
            && Equals(other);
        public override int GetHashCode() =>
            new HashCode()
            .Append(_update, _value)
            .CurrentHash;

        public static bool operator ==(EditableValue<T> left, EditableValue<T> right) =>
            left.Equals(right);
        public static bool operator !=(EditableValue<T> left, EditableValue<T> right) =>
            !left.Equals(right);

        public static bool operator ==(EditableValue<T> left, T right) =>
            left.Equals(right);
        public static bool operator !=(EditableValue<T> left, T right) =>
            !left.Equals(right);

        public static bool operator ==(T left, EditableValue<T> right) =>
            right.Equals(left);
        public static bool operator !=(T left, EditableValue<T> right) =>
            !right.Equals(left);

        #endregion
    }

    public static class EditableValue
    {
        public static EditableValue<T> NoAction<T>(T value = default(T)) =>
            EditableValue<T>.NoAction();
        public static EditableValue<T> Update<T>(T value) =>
            EditableValue<T>.Update(value);

        public static Type GetUnderlyingType(Type type) =>
            type != null
            && type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(EditableValue<>)
            ? type.GetGenericArguments()[0]
            : null;
    }
}
