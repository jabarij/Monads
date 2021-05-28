namespace Monads.FSharp

module Maybe =
    let toOption<'a> (maybe: Monads.Maybe<'a>): 'a option =
        maybe.Match(
            System.Func<'a, 'a option>(Some),
            System.Func<'a option>(fun _ -> None))

    let fromOption<'a> (option: 'a option): Monads.Maybe<'a> =
        match option with
        | Some e -> Monads.Maybe<'a>.Some(e)
        | None -> Monads.Maybe<'a>.None()