using System;
using System.Diagnostics;
using DotNetExtensions;

namespace Monads.Extensions;

[DebuggerStepThrough]
public static class ResultExtensions
{
    public static bool IsOk<T, TError>(this Result<T, TError> result) =>
        result.Match(
            ok: _ => true,
            error: _ => false);

    public static bool IsError<T, TError>(this Result<T, TError> result) =>
        result.Match(
            ok: _ => false,
            error: _ => true);

    public static T Reduce<T, TError>(this Result<T, TError> result, Func<TError, T> errorReduction) =>
        result.Match(
            ok: Functions.Id,
            error: errorReduction);

    public static TError ReduceError<T, TError>(this Result<T, TError> result, Func<T, TError> okReduction = null) =>
        result.Match(
            ok: okReduction ?? Functions.ReturnDefault<T, TError>,
            error: Functions.Id);

    public static T ReduceOrThrow<T, TError, TException>(this Result<T, TError> result, Func<TError, TException> getException) where TException : Exception =>
        result.Match(
            ok: Functions.Id,
            error: err => throw getException(err));

    public static T ReduceOrThrow<T, TException>(this Result<T, TException> result) where TException : Exception =>
        result.Match(
            ok: Functions.Id,
            error: ex => throw ex);

    public static void Act<TOk, TError>(this Result<TOk, TError> result, Action<TOk> ok, Action<TError> error) =>
        result.Match(
            ok: e => ok.ToVoidFunc().Invoke(e),
            error: e => error.ToVoidFunc().Invoke(e));

    public static Result<TOk, TError> Flatten<TOk, TError>(this Result<Result<TOk, TError>, TError> result) =>
        result.Match(
            ok: Functions.Id,
            error: Result<TOk, TError>.Error);

    public static Result<TOk, TError> Flatten<TOk, TError>(this Result<TOk, Result<TOk, TError>> result) =>
        result.Match(
            ok: Result<TOk, TError>.Ok,
            error: Functions.Id);

    public static Result<TOk, TError> Flatten<TOk, TError>(this Result<Result<TOk, TError>, Result<TOk, TError>> result) =>
        result.Match(
            ok: Functions.Id,
            error: Functions.Id);

    public static Result<TOk, Either<TErrorOuter, TErrorInner>> Flatten<TOk, TErrorOuter, TErrorInner>(this Result<Result<TOk, TErrorInner>, TErrorOuter> result) =>
        result.Match(
            ok: inner => inner.Match(
                ok: ok => Result<TOk, Either<TErrorOuter, TErrorInner>>.Ok(ok),
                error: err => Either<TErrorOuter, TErrorInner>.Right(err)),
            error: err => Either<TErrorOuter, TErrorInner>.Left(err));


    public static Result<TOk, Either<TErrorOuter, TErrorInner>> Bind<TOk, TErrorOuter, TErrorInner>(this Result<TOk, TErrorOuter> result, Func<TOk, Result<TOk, TErrorInner>> binder) =>
        result.Match(
            ok: ok => binder(ok).Match(
                stillOk => Result<TOk, Either<TErrorOuter, TErrorInner>>.Ok(stillOk),
                innerError => Result<TOk, Either<TErrorOuter, TErrorInner>>.Error(innerError)),
            error: outerError => Either<TErrorOuter, TErrorInner>.Left(outerError));

    public static Type GetOkType<TOk, TError>(this Result<TOk, TError> _) =>
        typeof(TOk);

    public static Type GetErrorType<TOk, TError>(this Result<TOk, TError> _) =>
        typeof(TError);
}