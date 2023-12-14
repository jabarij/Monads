using System;
using System.Diagnostics;

namespace Monads;

[DebuggerStepThrough]
[DebuggerDisplay("{DebuggerDisplay}")]
public readonly struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>
{
    private string DebuggerDisplay =>
        Match(
            left: e => $"left({e?.ToString() ?? "[null]"})",
            right: e => $"right({e?.ToString() ?? "[null]"})");

    private readonly bool _isRight;
    private readonly TLeft? _left;
    private readonly TRight? _right;

    public Either(TLeft left)
    {
        _left = left;
        _right = default;
        _isRight = false;
    }
    public Either(TRight right)
    {
        _left = default;
        _right = right;
        _isRight = true;
    }

    public T Match<T>(
        Func<TLeft, T> left,
        Func<TRight, T> right) =>
        _isRight
            ? right(_right)
            : left(_left);

    public Either<TLeftProjection, TRightProjection> Map<TLeftProjection, TRightProjection>(
        Func<TLeft, TLeftProjection> leftMapping,
        Func<TRight, TRightProjection> rightMapping) =>
        _isRight
            ? new Either<TLeftProjection, TRightProjection>(rightMapping(_right))
            : new Either<TLeftProjection, TRightProjection>(leftMapping(_left));

    public static Either<TLeft, TRight> Left(TLeft left) => new(left);

    public static Either<TLeft, TRight> Right(TRight right) => new(right);

    public static implicit operator Either<TLeft, TRight>(TLeft left) =>
        Left(left);

    public static implicit operator Either<TLeft, TRight>(TRight right) =>
        Right(right);

    #region Boiler-plate code

    public override string ToString()
    {
        string optionDesc =
            _isRight
                ? "right"
                : "left";
        string valueDesc =
            _isRight
                ? _right?.ToString()
                : _left?.ToString();
        string typeDesc =
            valueDesc is null
                ? string.Concat("<", _isRight ? typeof(TRight).Name : typeof(TLeft).Name, ">")
                : string.Empty;
        return string.Concat(optionDesc, typeDesc, "(", valueDesc ?? "[null]", ")");
    }

    public bool Equals(Either<TLeft, TRight> other) =>
        !(_isRight ^ other._isRight)
        && (_isRight
            ? Equals(_right, other._right)
            : Equals(_left, other._left));
    public override bool Equals(object? obj) =>
        obj is Either<TLeft, TRight> other
        && Equals(other);
    public override int GetHashCode() =>
        _isRight
            ? HashCode.Combine(_right)
            : HashCode.Combine(_left);

    public static bool operator ==(
        Either<TLeft, TRight> left,
        Either<TLeft, TRight> right) =>
        left.Equals(right);

    public static bool operator !=(
        Either<TLeft, TRight> left,
        Either<TLeft, TRight> right) =>
        !left.Equals(right);

    #endregion
}

public static class Either
{
    public static Type GetUnderlyingLeftType(Type eitherType) =>
        eitherType.IsGenericType
        && eitherType.GetGenericTypeDefinition() == typeof(Either<,>)
            ? eitherType.GetGenericArguments()[0]
            : null;

    public static Type GetUnderlyingRightType(Type eitherType) =>
        eitherType.IsGenericType
        && eitherType.GetGenericTypeDefinition() == typeof(Either<,>)
            ? eitherType.GetGenericArguments()[1]
            : null;
}