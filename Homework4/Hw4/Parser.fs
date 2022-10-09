module Hw4.Parser

open System
open Hw4.CalculatorOperation
open CalcOptions

let isArgLengthSupported (args: string[]) = args.Length = 3

let parseOperation arg =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException() |> raise

let parseDouble (str:string) =
    match Double.TryParse str  with
    | true, double -> double
    | _ -> ArgumentException() |> raise
    
let parseCalcArguments args =
   if args = null || not(isArgLengthSupported args) then
       ArgumentException() |> raise
   { arg1 = (parseDouble args[0])
     operation = parseOperation args[1]
     arg2 = (parseDouble args[2]) }