module Hw5.Program

open Hw5
open Hw5.Parser
open Hw5.Calculator

let getString error= 
    match error with 
    | Message.WrongArgLength -> "Wrong number of arguments"
    | Message.WrongArgFormat -> "Numbers can't be parsed"
    | Message.WrongArgFormatOperation -> "Wrong operator"
    | Message.DivideByZero -> "0-division is impossible"
    | _ -> "Unknown error"

[<EntryPoint>]
let main (args: string[]) =
    match parseCalcArguments args with 
    | Ok expression -> printf $"{calculateTuple expression}"
    | Error err -> printf $"Error occured: {getString err}"
    0