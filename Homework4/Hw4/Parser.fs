module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions =
    { arg1: float
      arg2: float
      operation: CalculatorOperation }

let isArgLengthSupported (args: string []) = args.Length = 3

let parseOperation (arg: string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException() |>raise

let parseDouble (str:string) =
    match Double.TryParse str  with
    | true,double -> Some double
    | _ -> ArgumentException()|>raise



let parseCalcArguments (args: string []) =
   if args = null || not(isArgLengthSupported args)
   then ArgumentException()|> raise
   else 
    {arg1 = (parseDouble args[0]).Value
     operation = parseOperation args[1]
     arg2 = (parseDouble args[2]).Value}