using DotNetExtensions;
using System;

namespace Monads
{
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay}")]
    public struct Result<TOk, TError> : IEquatable<Result<TOk, TError>>
    {
        private string DebuggerDisplay =>
            Match(
                ok: e => $"ok({e?.ToString() ?? "[null]"})",
                error: e => $"error({e?.ToString() ?? "[null]"})");

        private readonly bool _isOk;
        private readonly TOk _ok;
        private readonly TError _error;

        public Result(TOk ok)
        {
            _ok = ok;
            _error = default(TError);
            _isOk = true;
        }
        public Result(TError error)
        {
            _ok = default(TOk);
            _error = error;
            _isOk = false;
        }

        public Result<TOkResult, TError> Bind<TOkResult>(Func<TOk, Result<TOkResult, TError>> binder) =>
            _isOk
            ? binder(_ok)
            : Result<TOkResult, TError>.Error(_error);

        public TResult Match<TResult>(
            Func<TOk, TResult> ok,
            Func<TError, TResult> error) =>
            _isOk
            ? ok(_ok)
            : error(_error);

        public Result<TOkProjection, TError> Map<TOkProjection>(
            Func<TOk, TOkProjection> mapping) =>
            _isOk
            ? new Result<TOkProjection, TError>(mapping(_ok))
            : new Result<TOkProjection, TError>(_error);

        public Result<TOk, TErrorProjection> MapError<TErrorProjection>(
            Func<TError, TErrorProjection> mapping) =>
            _isOk
            ? new Result<TOk, TErrorProjection>(_ok)
            : new Result<TOk, TErrorProjection>(mapping(_error));

        public static Result<TOk, TError> Ok(TOk ok) =>
            new Result<TOk, TError>(ok);

        public static Result<TOk, TError> Error(TError error) =>
            new Result<TOk, TError>(error);

        #region Boiler-plate code

        public override string ToString()
        {
            string optionDesc =
                _isOk
                ? "ok"
                : "error";
            string valueDesc =
                _isOk
                ? _ok?.ToString()
                : _error?.ToString();
            string typeDesc =
                valueDesc is null
                ? string.Concat("<", _isOk ? typeof(TOk).Name : typeof(TError).Name, ">")
                : string.Empty;
            return string.Concat(optionDesc, typeDesc, "(", valueDesc ?? "[null]", ")");
        }

        public bool Equals(Result<TOk, TError> other) =>
            !(_isOk ^ other._isOk)
            && (_isOk
                ? Equals(_ok, other._ok)
                : Equals(_error, other._error));
        public override bool Equals(object obj) =>
            obj is Result<TOk, TError> other
            && Equals(other);
        public override int GetHashCode() =>
            _isOk
            ? new HashCode().Append(_ok).CurrentHash
            : new HashCode().Append(_error).CurrentHash;

        public static bool operator ==(Result<TOk, TError> left, Result<TOk, TError> right) =>
            left.Equals(right);

        public static bool operator !=(Result<TOk, TError> left, Result<TOk, TError> right) =>
            !left.Equals(right);

        public static implicit operator Result<TOk, TError>(TOk ok) =>
            new Result<TOk, TError>(ok);

        public static implicit operator Result<TOk, TError>(TError error) =>
            new Result<TOk, TError>(error);

        public static implicit operator Result<TOk, TError>(ResultOk<TOk> ok) =>
            Ok(ok.Result);

        public static implicit operator Result<TOk, TError>(ResultError<TError> error) =>
            Error(error.Result);

        #endregion
    }

    public static class Result
    {
        public static ResultOk<TOk> Ok<TOk>(TOk ok) =>
            new ResultOk<TOk>(ok);

        public static ResultError<TError> Error<TError>(TError error) =>
            new ResultError<TError>(error);

        public static Type GetUnderlyingOkType(Type eitherType) =>
            eitherType.IsGenericType
            && eitherType.GetGenericTypeDefinition() == typeof(Result<,>)
            ? eitherType.GetGenericArguments()[0]
            : null;

        public static Type GetUnderlyingErrorType(Type eitherType) =>
            eitherType.IsGenericType
            && eitherType.GetGenericTypeDefinition() == typeof(Result<,>)
            ? eitherType.GetGenericArguments()[1]
            : null;
    }
}