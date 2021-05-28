namespace Monads.DataOps.FSharp

type ClearableValue<'value> =
    | Set of 'value
    | Clear
    | NoAction

module ClearableValue =
    let toFSharp<'value> (clearable: Monads.DataOps.ClearableValue<'value>): ClearableValue<'value> =
        clearable.Match(
            System.Func<'value, ClearableValue<'value>>(Set),
            System.Func<ClearableValue<'value>>(fun _ -> Clear),
            System.Func<ClearableValue<'value>>(fun _ -> NoAction))

    let toCSharp<'value> (clearable: ClearableValue<'value>): Monads.DataOps.ClearableValue<'value> =
        match clearable with
        | Set value -> Monads.DataOps.ClearableValue<'value>.Set(value)
        | Clear -> Monads.DataOps.ClearableValue<'value>.Clear()
        | NoAction -> Monads.DataOps.ClearableValue<'value>.NoAction()

    let map (mapper: 'a -> 'b) (clearable: ClearableValue<'a>): ClearableValue<'b> =
        match clearable with
        | Set value -> Set (mapper value)
        | Clear -> Clear
        | NoAction -> NoAction
