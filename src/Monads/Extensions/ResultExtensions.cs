using DotNetExtensions;
using System;

namespace Monads.Extensions
{
    [System.Diagnostics.DebuggerStepThrough]
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

        public static Type GetOkType<TOk, TError>(this Result<TOk, TError> _) =>
            typeof(TOk);

        public static Type GetErrorType<TOk, TError>(this Result<TOk, TError> _) =>
            typeof(TError);
    }
}
