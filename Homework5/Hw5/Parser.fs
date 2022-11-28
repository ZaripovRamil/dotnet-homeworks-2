module Hw5.Parser

open System
open System.Globalization
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args: string []) =
    match args = null with
    | false ->
        match args.Length with
        | 3 -> Ok args
        | _ -> Error Message.WrongArgLength
    | true -> Error Message.WrongArgLength

let parseOp str =
    match str with
    | Plus -> Some CalculatorOperation.Plus
    | Minus -> Some CalculatorOperation.Minus
    | Multiply -> Some CalculatorOperation.Multiply
    | Divide -> Some CalculatorOperation.Divide
    | _ -> None

let parseDouble (str: string) =
    match Double.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) with
    | true, double -> Some double
    | _ -> None

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let isOperationSupported arg1 operation arg2=
    match parseOp operation with
    | Some op -> Ok(arg1, op, arg2)
    | None -> Error Message.WrongArgFormatOperation

let parseArgs (args: string []) =
    match parseDouble args[0] with
    | None -> Error Message.WrongArgFormat
    | Some a ->
        match parseDouble args[2] with
        | None -> Error Message.WrongArgFormat
        | Some b -> isOperationSupported a args[1] b

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let isDividingByZero arg1 operation arg2 =
    match operation with
    | CalculatorOperation.Divide ->
        match arg2 with
        | 0.0 -> Error Message.DivideByZero
        | _ -> Ok(arg1, operation, arg2)
    | _ -> Ok(arg1, operation, arg2)

let parseCalcArguments args =
    maybe {
        let! parseable = args |> isArgLengthSupported
        let! correctArgs = parseable |> parseArgs
        let! checkDivisionByZero = correctArgs |||> isDividingByZero
        return checkDivisionByZero
    }
