module Hw5.Program

open Hw5
open Hw5.Parser
open Hw5.Calculator

let getString error =
    match error with
    | Message.WrongArgLength -> "Wrong number of arguments"
    | Message.WrongArgFormat -> "Numbers can't be parsed"
    | Message.WrongArgFormatOperation -> "Wrong operator"
    | Message.DivideByZero -> "0-division is impossible"
    | _ -> "Unknown error"

let successfulResult expression =
    expression |||> calculate |> printf "%f"
    0

let failedResult err =
    printf $"Error occured: {getString err}"
    int err

[<EntryPoint>]
let main args =
    match parseCalcArguments args with
    | Ok expression -> successfulResult expression
    | Error err -> failedResult err
