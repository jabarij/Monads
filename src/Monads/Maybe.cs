using System;
using System.Diagnostics;

namespace Monads;

[DebuggerDisplay("{DebuggerDisplay}")]
public readonly struct Maybe<T> : IEquatable<Maybe<T>>
{
    private string DebuggerDisplay =>
        Match(
            some: e => $"some({e})",
            none: () => $"none<{typeof(T).Name}>");

    private readonly T _value;
    private readonly bool _isSome;

    public Maybe(T value)
    {
        _value = value;
        _isSome = true;
    }

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> binder) =>
        _isSome
            ? binder(_value)
            : Maybe<TResult>.None();

    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) =>
        _isSome
            ? some(_value)
            : none();

    public Maybe<TProjection> Map<TProjection>(Func<T, TProjection> mapping) =>
        _isSome
            ? new Maybe<TProjection>(mapping(_value))
            : new Maybe<TProjection>();

    public static Maybe<T> None() => new();

    public static Maybe<T> Some(T value) => new(value);

    #region Boiler-plate code

    public override string ToString()
    {
        var valueDesc =
            _isSome
                ? _value?.ToString()
                : null;
        string typeDesc =
            valueDesc is null
                ? string.Concat("<", typeof(T).Name, ">")
                : string.Empty;
        return _isSome
            ? string.Concat("some", typeDesc, "(", valueDesc ?? "[null]", ")")
            : string.Concat("none", typeDesc);
    }

    public bool Equals(Maybe<T> other) =>
        _isSome == other._isSome
        && (!_isSome || Equals(_value, other._value));
    public override bool Equals(object? obj) =>
        obj is Maybe<T> other
        && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(_value);

    public static bool operator ==(Maybe<T> left, Maybe<T> right) =>
        left.Equals(right);
    public static bool operator !=(Maybe<T> left, Maybe<T> right) =>
        !left.Equals(right);

    public static implicit operator Maybe<T>(T value) =>
        new(value);

    public static explicit operator T(Maybe<T> maybe) =>
        maybe.Match(
            some: Functions.Id,
            none: () => throw new InvalidOperationException("Cannot reduce None to raw value."));

    #endregion
}

public static class Maybe
{
    public static Maybe<T> Some<T>(T value) =>
        Maybe<T>.Some(value);

    public static Maybe<T> None<T>(T? _ = default) =>
        Maybe<T>.None();

    public static Maybe<T> Resolve<T>(T? value) where T : struct =>
        value.HasValue ? Some(value.Value) : None(default(T));

    public static Maybe<T> Resolve<T>(T? value) where T : class =>
        value is not null ? Some(value) : None<T>();

    public static Type? GetUnderlyingType(Type maybeType) =>
        maybeType.IsGenericType
        && maybeType.GetGenericTypeDefinition() == typeof(Maybe<>)
            ? maybeType.GetGenericArguments()[0]
            : null;
}