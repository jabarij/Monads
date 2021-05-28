namespace Monads.FSharp

module Result =
    let toFSharp<'ok, 'error> (result: Monads.Result<'ok, 'error>): Result<'ok, 'error> =
        result.Match(
            System.Func<'ok, Result<'ok, 'error>>(Ok),
            System.Func<'error, Result<'ok, 'error>>(Error))

    let toCSharp<'ok, 'error> (result: Result<'ok, 'error>): Monads.Result<'ok, 'error> =
        match result with
        | Ok ok -> Monads.Result<'ok, 'error>.Ok(ok)
        | Error error -> Monads.Result<'ok, 'error>.Error(error)