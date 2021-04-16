using DotNetExtensions;
using System;

namespace Monads.DataOps
{
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
    public struct ClearableValue<T> : IEquatable<ClearableValue<T>>, IEquatable<T>
    {
        private string DebuggerDisplay =>
            Match(
                set: e => $"set({e})",
                clear: () => $"clear<{typeof(T).FullName}>",
                noAction: () => $"noAction<{typeof(T).FullName}>");

        private readonly T _value;
        private readonly bool _set;
        private readonly bool _clear;

        public ClearableValue(T value)
        {
            _value = value;
            _set = true;
            _clear = false;
        }
        private ClearableValue(object _)
        {
            _value = default(T);
            _set = false;
            _clear = true;
        }

        public TResult Match<TResult>(Func<T, TResult> set, Func<TResult> clear, Func<TResult> noAction) =>
            _set ? set(_value)
            : _clear ? clear()
            : noAction();

        public ClearableValue<TResult> Map<TResult>(Func<T, TResult> map) =>
            _set ? new ClearableValue<TResult>(map(_value))
            : _clear ? new ClearableValue<TResult>(_: null)
            : new ClearableValue<TResult>();

        public static ClearableValue<T> NoAction() =>
            new ClearableValue<T>();
        public static ClearableValue<T> Set(T value) =>
            new ClearableValue<T>(value);
        public static ClearableValue<T> Clear() =>
            new ClearableValue<T>(_: null);

        #region Boiler-plate code

        public bool Equals(T value) =>
            Equals(_value, value);
        public bool Equals(ClearableValue<T> other) =>
            _set == other._set
            && _clear == other._clear
            && (!_set || Equals(_value, other._value));
        public override bool Equals(object obj) =>
            obj is ClearableValue<T> other
            && Equals(other);
        public override int GetHashCode() =>
            new HashCode()
            .Append(_set, _clear, _value)
            .CurrentHash;

        public static bool operator ==(ClearableValue<T> left, ClearableValue<T> right) =>
            left.Equals(right);
        public static bool operator !=(ClearableValue<T> left, ClearableValue<T> right) =>
            !left.Equals(right);

        public static bool operator ==(ClearableValue<T> left, T right) =>
            left.Equals(right);
        public static bool operator !=(ClearableValue<T> left, T right) =>
            !left.Equals(right);

        public static bool operator ==(T left, ClearableValue<T> right) =>
            right.Equals(left);
        public static bool operator !=(T left, ClearableValue<T> right) =>
            !right.Equals(left);

        #endregion
    }

    public static class ClearableValue
    {
        public static ClearableValue<T> NoAction<T>(T _ = default(T)) => ClearableValue<T>.NoAction();
        public static ClearableValue<T> Set<T>(T value) => ClearableValue<T>.Set(value);
        public static ClearableValue<T> Clear<T>() => ClearableValue<T>.Clear();

        public static Type GetUnderlyingType(Type type) =>
            type != null
            && type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(ClearableValue<>)
            ? type.GetGenericArguments()[0]
            : null;
    }
}