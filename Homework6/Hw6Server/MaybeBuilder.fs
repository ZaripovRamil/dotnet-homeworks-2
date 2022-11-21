module Hw6.MaybeBuilder

type MaybeBuilder() =
    member this.Bind(x, f) =
        match x with
        | Error error -> Error error
        | Ok a -> f a

    member this.Return(x) = Ok x

let maybe = MaybeBuilder()
