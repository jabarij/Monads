using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Monads;

[DebuggerStepThrough]
[DebuggerDisplay("{DebuggerDisplay}")]
public readonly struct Result<TOk, TError> : IEquatable<Result<TOk, TError>>
{
    private string DebuggerDisplay =>
        Match(
            ok: e => $"ok({e?.ToString() ?? "[null]"})",
            error: e => $"error({e?.ToString() ?? "[null]"})"
        );

    [MemberNotNullWhen(true, nameof(OkValue))]
    [MemberNotNullWhen(false, nameof(ErrorValue))]
    private bool IsOk { get; }

    private TOk? OkValue { get; }
    private TError? ErrorValue { get; }

    public Result(TOk okValue)
    {
        OkValue = okValue;
        ErrorValue = default;
        IsOk = true;
    }

    public Result(TError errorValue)
    {
        OkValue = default;
        ErrorValue = errorValue;
        IsOk = false;
    }

    [Pure]
    public Result<TOkResult, TError> Bind<TOkResult>(
        [Pure] Func<TOk, Result<TOkResult, TError>> binder) =>
        IsOk
            ? binder(OkValue)
            : Result<TOkResult, TError>.Error(ErrorValue);

    [Pure]
    public TResult Match<TResult>(
        [Pure] Func<TOk, TResult> ok,
        [Pure] Func<TError, TResult> error) =>
        IsOk
            ? ok(OkValue)
            : error(ErrorValue);

    [Pure]
    public Result<TOkProjection, TError> Map<TOkProjection>(
        [Pure] Func<TOk, TOkProjection> mapping) =>
        IsOk
            ? new Result<TOkProjection, TError>(mapping(OkValue))
            : new Result<TOkProjection, TError>(ErrorValue);

    [Pure]
    public Result<TOk, TErrorProjection> MapError<TErrorProjection>(
        [Pure] Func<TError, TErrorProjection> mapping) =>
        IsOk
            ? new Result<TOk, TErrorProjection>(OkValue)
            : new Result<TOk, TErrorProjection>(mapping(ErrorValue));
   
    [Pure]
    public static Result<TOk, TError> Ok(TOk ok) => new(ok);

    [Pure]
    public static Result<TOk, TError> Error(TError error) => new(error);

    #region Boiler-plate code

    public override string ToString()
    {
        var optionDesc =
            IsOk
                ? "ok"
                : "error";
        var valueDesc =
            IsOk
                ? OkValue?.ToString()
                : ErrorValue?.ToString();
        string typeDesc =
            valueDesc is null
                ? string.Concat("<", IsOk ? typeof(TOk).Name : typeof(TError).Name, ">")
                : string.Empty;
        return string.Concat(
            optionDesc,
            typeDesc,
            "(",
            valueDesc ?? "[null]",
            ")"
        );
    }

    public bool Equals(Result<TOk, TError> other) =>
        !(IsOk ^ other.IsOk)
        && (IsOk
            ? Equals(OkValue, other.OkValue)
            : Equals(ErrorValue, other.ErrorValue));

    public override bool Equals(object? obj) =>
        obj is Result<TOk, TError> other
        && Equals(other);

    public override int GetHashCode() =>
        IsOk
            ? HashCode.Combine(OkValue)
            : HashCode.Combine(ErrorValue);

    public static bool operator ==(Result<TOk, TError> left, Result<TOk, TError> right) =>
        left.Equals(right);

    public static bool operator !=(Result<TOk, TError> left, Result<TOk, TError> right) =>
        !left.Equals(right);

    public static implicit operator Result<TOk, TError>(TOk ok) =>
        new(ok);

    public static implicit operator Result<TOk, TError>(TError error) =>
        new(error);

    public static implicit operator Result<TOk, TError>(ResultOk<TOk> ok) =>
        Ok(ok.Result);

    public static implicit operator Result<TOk, TError>(ResultError<TError> error) =>
        Error(error.Result);

    #endregion
}

public static class Result
{
    public static ResultOk<TOk> Ok<TOk>(TOk ok) => new(ok);

    public static ResultError<TError> Error<TError>(TError error) => new(error);

    public static Type? GetUnderlyingOkType(Type eitherType) =>
        eitherType.IsGenericType
        && eitherType.GetGenericTypeDefinition() == typeof(Result<,>)
            ? eitherType.GetGenericArguments()[0]
            : null;

    public static Type? GetUnderlyingErrorType(Type eitherType) =>
        eitherType.IsGenericType
        && eitherType.GetGenericTypeDefinition() == typeof(Result<,>)
            ? eitherType.GetGenericArguments()[1]
            : null;
}