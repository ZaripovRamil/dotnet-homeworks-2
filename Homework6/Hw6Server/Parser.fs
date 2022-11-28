module Hw6.Parser

open System
open System.Globalization
open Hw6.Calculator
open Hw6.MaybeBuilder

let parseOp str =
    match str with
    | Plus |"Plus"-> Some CalculatorOperation.Plus
    | Minus | "Minus"-> Some CalculatorOperation.Minus
    | Multiply|"Multiply" -> Some CalculatorOperation.Multiply
    | Divide | "Divide"-> Some CalculatorOperation.Divide
    | _ -> None

let parseDouble (str: string) =
    match Double.TryParse (str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture) with
    | true, double -> Some double
    | _ -> None

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let isOperationSupported arg1 operation arg2=
    match parseOp operation with
    | Some op -> Ok(arg1, op, arg2)
    | None -> Error $"Could not parse value '{operation}'"

let parseArgs (args: string []) =
    match parseDouble args[0] with
    | None -> Error $"Could not parse value '{args[0]}'"
    | Some a ->
        match parseDouble args[2] with
        | None -> Error $"Could not parse value '{args[2]}'"
        | Some b -> isOperationSupported a args[1] b

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let isDividingByZero arg1 operation arg2 =
    match operation with
    | CalculatorOperation.Divide ->
        match arg2 with
        | 0.0 -> Error "DivideByZero"
        | _ -> Ok(arg1, operation, arg2)
    | _ -> Ok(arg1, operation, arg2)

let parseCalcArguments args =
    maybe {
        let! correctArgs = args |> parseArgs
        let! checkDivisionByZero = correctArgs |||> isDividingByZero
        return checkDivisionByZero
    }
