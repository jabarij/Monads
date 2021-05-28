namespace Monads.DataOps.FSharp

type EditableValue<'value> =
    | Update of 'value
    | NoAction

module EditableValue =
    let toFSharp<'value> (editable: Monads.DataOps.EditableValue<'value>): EditableValue<'value> =
        editable.Match(
            System.Func<'value, EditableValue<'value>>(Update),
            System.Func<EditableValue<'value>>(fun _ -> NoAction))

    let toCSharp<'value> (editable: EditableValue<'value>): Monads.DataOps.EditableValue<'value> =
        match editable with
        | Update value -> Monads.DataOps.EditableValue<'value>.Update(value)
        | NoAction -> Monads.DataOps.EditableValue<'value>.NoAction()

    let map (mapper: 'a -> 'b) (editable: EditableValue<'a>): EditableValue<'b> =
        match editable with
        | Update value -> Update (mapper value)
        | NoAction -> NoAction
