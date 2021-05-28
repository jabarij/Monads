namespace Monads.FSharp

type Either<'left, 'right> =
    | Left of 'left
    | Right of 'right

module Either =
    let toFSharp<'left, 'right> (result: Monads.Either<'left, 'right>): Either<'left, 'right> =
        result.Match(System.Func<'left, Either<'left, 'right>>(Left), System.Func<'right, Either<'left, 'right>>(Right))

    let toCSharp<'left, 'right> (result: Either<'left, 'right>): Monads.Either<'left, 'right> =
        match result with
        | Left left -> Monads.Either<'left, 'right>.Left(left)
        | Right right -> Monads.Either<'left, 'right>.Right(right)

    let mapLeft (mapper: 'a -> 'b) (either: Either<'a, _>): Either<'b, _> =
        match either with
        | Left left -> Left (mapper left)
        | Right right -> Right right
        
    let mapRight (mapper: 'a -> 'b) (either: Either<_, 'a>): Either<_, 'b> =
        match either with
        | Left left -> Left left
        | Right right -> Right (mapper right)

    let map (leftMapper: 'left -> 'leftProjection) (rightMapper: 'right -> 'rightProjection) (either: Either<'left, 'right>): Either<'leftProjection, 'rightProjection> =
        match either with
        | Left left -> Left (leftMapper left)
        | Right right -> Right (rightMapper right)